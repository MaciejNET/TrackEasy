using Shouldly;
using TrackEasy.Domain.Cities;
using TrackEasy.Domain.Coaches;
using TrackEasy.Domain.Connections;
using TrackEasy.Domain.Discounts;
using TrackEasy.Domain.Operators;
using TrackEasy.Domain.Shared;
using TrackEasy.Domain.Stations;
using TrackEasy.Domain.Trains;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.UnitTests.Connection;

public class ConnectionTests
{
    // Helper methods for creating coaches and trains
    private static Coach CreateCoach(string code, int seatCount = 1, Guid? operatorId = null)
    {
        var seats = Enumerable.Range(1, seatCount)
            .Select(seatNumber => Seat.Create(seatNumber))
            .ToList();

        return Coach.Create(code, seats, operatorId ?? Guid.NewGuid());
    }

    private static Train CreateTrain(string name, Operator op)
    {
        op.AddCoach("COACH1", [1, 2]);
        var coach = op.Coaches.First();
        op.AddTrain(name, [(coach.Id, 1)]);
        var train = op.Trains[0];
        return train;
    }

    [Fact]
    public void Create_Should_CreateConnection_For_ValidData()
    {
        // Arrange
        var op = Operator.Create("OPERATOR", "OP");
        var price = new Money(1.5m, Currency.EUR);
        var train = CreateTrain("OP-1234", op);
        var schedule = new Schedule(DateOnly.MinValue, DateOnly.MaxValue, [DayOfWeek.Monday]);
        var stations = CreateTwoStations();

        // Act
        var conn = Domain.Connections.Connection.Create("OP-C012", op, price, train, schedule, stations, true);

        // Assert
        conn.ShouldNotBeNull();
        conn.Id.ShouldNotBe(Guid.Empty);
        conn.Name.ShouldBe("OP-C012");
        conn.Operator.ShouldBe(op);
        conn.PricePerKilometer.ShouldBe(price);
        conn.Train.ShouldBe(train);
        conn.Schedule.ShouldBe(schedule);
        conn.Stations.ShouldBe(stations);
        conn.NeedsSeatReservation.ShouldBeTrue();
        conn.IsActivated.ShouldBeFalse();
        conn.Request.ShouldNotBeNull();
        conn.Request!.RequestType.ShouldBe(ConnectionRequestType.ADD);
        conn.DomainEvents.Count.ShouldBe(1);
        conn.DomainEvents.First().ShouldBeOfType<ConnectionRequestCreatedEvent>();
    }

    [Fact]
    public void Update_Should_ChangeNameAndPrice()
    {
        var conn = CreateSampleConnection();
        conn.Update("OP-NEW1", new Money(2m, Currency.EUR));
        conn.Name.ShouldBe("OP-NEW1");
        conn.PricePerKilometer.ShouldBe(new Money(2m, Currency.EUR));
    }

    [Fact]
    public void UpdateSchedule_Should_QueueUpdateRequest_And_AddEvent()
    {
        var conn = CreateSampleConnection();
        var newSchedule = new Schedule(DateOnly.MinValue, DateOnly.MaxValue, [DayOfWeek.Tuesday]);
        var newStations = CreateTwoStations();

        conn.UpdateSchedule(newSchedule, newStations);

        conn.Request.ShouldNotBeNull();
        conn.Request!.RequestType.ShouldBe(ConnectionRequestType.UPDATE);
        conn.DomainEvents.Last().ShouldBeOfType<ConnectionRequestCreatedEvent>();
    }

    [Fact]
    public void Delete_Should_QueueDeleteRequest_And_AddEvent()
    {
        var conn = CreateSampleConnection();
        conn.Delete();
        conn.Request.ShouldNotBeNull();
        conn.Request!.RequestType.ShouldBe(ConnectionRequestType.DELETE);
        conn.DomainEvents.Last().ShouldBeOfType<ConnectionRequestCreatedEvent>();
    }

    [Fact]
    public void ApproveRequest_ForAdd_Should_ActivateAndRaiseApprovedEvent()
    {
        var conn = CreateSampleConnection();
        conn.ApproveRequest();

        conn.IsActivated.ShouldBeTrue();
        conn.Request.ShouldBeNull();
        conn.DomainEvents.Any(e => e is ConnectionRequestApprovedEvent).ShouldBeTrue();
    }

    [Fact]
    public void RejectRequest_ForAdd_Should_RaiseRejectedAndDeletedEvents()
    {
        var conn = CreateSampleConnection();
        conn.ClearDomainEvents();
        conn.RejectRequest();

        conn.Request.ShouldBeNull();
        conn.DomainEvents.Count.ShouldBe(2);
        conn.DomainEvents.First().ShouldBeOfType<ConnectionRequestRejectedEvent>();
        conn.DomainEvents.Last().ShouldBeOfType<ConnectionDeletedEvent>();
    }

    [Theory]
    [InlineData(DayOfWeek.Monday, true)]
    [InlineData(DayOfWeek.Tuesday, false)]
    public void IsConnectionRunning_ReturnsExpected_For_Days(DayOfWeek day, bool expected)
    {
        var op = Operator.Create("OPERATOR", "OP");
        var train = CreateTrain("OP-1234", op);
        var conn = Domain.Connections.Connection.Create(
            "OP-X123",
            op,
            new Money(1, Currency.USD),
            train,
            new Schedule(DateOnly.MinValue, DateOnly.MaxValue, [DayOfWeek.Monday]),
            CreateTwoStations(),
            false);

        conn.ApproveRequest();
        conn.IsConnectionRunning(new DateOnly(2024, 1, (int)day)).ShouldBe(expected);
    }

    [Fact]
    public void CalculatePrice_WithoutDiscount_ReturnsDistanceTimesRate()
    {
        var conn = CreateSampleConnection();
        conn.ApproveRequest();
        var s1 = conn.Stations[0].Station;
        var s2 = conn.Stations[1].Station;
        var dist = s1.GeographicalCoordinates.CalculateDistanceTo(s2.GeographicalCoordinates);
        var expected = new Money(conn.PricePerKilometer.Amount * (decimal)dist, conn.PricePerKilometer.Currency);

        var price = conn.CalculatePrice(s1.Id, s2.Id, null);
        price.ShouldBe(expected);
    }

    [Fact]
    public void CalculatePrice_WithDiscount_AppliesPercentage()
    {
        var conn = CreateSampleConnection();
        conn.ApproveRequest();
        var s1 = conn.Stations[0].Station;
        var s2 = conn.Stations[1].Station;
        var dist = s1.GeographicalCoordinates.CalculateDistanceTo(s2.GeographicalCoordinates);
        var total = conn.PricePerKilometer.Amount * (decimal)dist;
        var discount = Discount.Create("D10", 10);
        var expected = new Money(total - total * 0.10m, conn.PricePerKilometer.Currency);

        conn.CalculatePrice(s1.Id, s2.Id, discount).ShouldBe(expected);
    }

    [Fact]
    public void CalculatePrice_Throws_WhenStationNotFound()
    {
        var conn = CreateSampleConnection();
        conn.ApproveRequest();
        var bad = Guid.NewGuid();

        var ex = Should.Throw<TrackEasyException>(() => conn.CalculatePrice(bad, bad, null));
        ex.Code.ShouldBe(SharedCodes.EntityNotFound);
    }

    [Fact]
    public void CalculatePrice_Throws_WhenDistanceNonPositive()
    {
        var s = Station.Create("Station", City.Create("name", Country.AD, []), new GeographicalCoordinates(0,0));
        var op = Operator.Create("OPERATOR", "OP");
        var train = CreateTrain("OP-1234", op);

        var stations = new[]
        {
            ConnectionStation.Create(s, null, TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)), 1),
            ConnectionStation.Create(s, TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)), null, 2)
        };
        
        var conn = Domain.Connections.Connection.Create(
            "OP-Z123",
            op,
            new Money(1, Currency.USD),
            train,
            new Schedule(DateOnly.MinValue, DateOnly.MaxValue, [DayOfWeek.Monday]),
            stations,
            false);
        conn.ApproveRequest();

        var ex = Should.Throw<TrackEasyException>(() => conn.CalculatePrice(s.Id, s.Id, null));
        ex.Code.ShouldBe(SharedCodes.InvalidInput);
    }

    private static ConnectionStation[] CreateTwoStations()
    {
        var s1 = Station.Create("Station1", City.Create("City1", Country.AD, []), new GeographicalCoordinates(0,0));
        var s2 = Station.Create("Station2", City.Create("City2", Country.AD, []), new GeographicalCoordinates(1,1));
        return
        [
            ConnectionStation.Create(s1, null, TimeOnly.FromTimeSpan(TimeSpan.FromHours(8)), 1),
            ConnectionStation.Create(s2, TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)), null, 2)
        ];
    }

    private static Domain.Connections.Connection CreateSampleConnection()
    {
        var op = Operator.Create("OPERATOR", "OP");
        var train = CreateTrain("OP-1234", op);
        return Domain.Connections.Connection.Create(
            "OP-C123",
            op,
            new Money(1m, Currency.USD),
            train,
            new Schedule(DateOnly.MinValue, DateOnly.MaxValue, [DayOfWeek.Monday, DayOfWeek.Tuesday]),
            CreateTwoStations(),
            false);
    }
}