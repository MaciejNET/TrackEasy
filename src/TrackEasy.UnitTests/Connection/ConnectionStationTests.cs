using FluentValidation;
using Shouldly;
using TrackEasy.Domain.Cities;
using TrackEasy.Domain.Connections;
using TrackEasy.Domain.Stations;

namespace TrackEasy.UnitTests.Connection;

public class ConnectionStationTests
{
    [Fact]
    public void CreateConnectionStation_WithValidArrivalAndDeparture_ShouldSucceed()
    {
        var station = GetValidStation();
        var arrival = new TimeOnly(8, 0);
        var departure = new TimeOnly(9, 0);
        const int sequenceNumber = 1;

        var connectionStation = ConnectionStation.Create(station, arrival, departure, sequenceNumber);

        connectionStation.Id.ShouldNotBe(Guid.Empty);
        connectionStation.Station.ShouldBe(station);
        connectionStation.ArrivalTime.ShouldBe(arrival);
        connectionStation.DepartureTime.ShouldBe(departure);
        connectionStation.SequenceNumber.ShouldBe(sequenceNumber);
    }

    [Fact]
    public void CreateConnectionStation_WithOnlyArrivalTime_ShouldSucceed()
    {
        var station = GetValidStation();
        var arrival = new TimeOnly(10, 0);
        const int sequenceNumber = 2;

        var connectionStation = ConnectionStation.Create(station, arrival, null, sequenceNumber);

        connectionStation.Id.ShouldNotBe(Guid.Empty);
        connectionStation.Station.ShouldBe(station);
        connectionStation.ArrivalTime.ShouldBe(arrival);
        connectionStation.DepartureTime.ShouldBeNull();
        connectionStation.SequenceNumber.ShouldBe(sequenceNumber);
    }

    [Fact]
    public void CreateConnectionStation_WithOnlyDepartureTime_ShouldSucceed()
    {
        var station = GetValidStation();
        TimeOnly? arrival = null;
        var departure = new TimeOnly(11, 0);
        const int sequenceNumber = 3;

        var connectionStation = ConnectionStation.Create(station, arrival, departure, sequenceNumber);

        connectionStation.Id.ShouldNotBe(Guid.Empty);
        connectionStation.Station.ShouldBe(station);
        connectionStation.ArrivalTime.ShouldBeNull();
        connectionStation.DepartureTime.ShouldBe(departure);
        connectionStation.SequenceNumber.ShouldBe(sequenceNumber);
    }

    [Fact]
    public void CreateConnectionStation_WithBothTimesNull_ShouldThrowValidationException()
    {
        var station = GetValidStation();
        int sequenceNumber = 4;

        Should.Throw<ValidationException>(() =>
            ConnectionStation.Create(station, null, null, sequenceNumber));
    }

    [Fact]
    public void CreateConnectionStation_WithArrivalAfterDeparture_ShouldThrowValidationException()
    {
        var station = GetValidStation();
        var arrival = new TimeOnly(12, 0);
        var departure = new TimeOnly(11, 0);
        const int sequenceNumber = 5;

        var exception = Should.Throw<ValidationException>(() =>
            ConnectionStation.Create(station, arrival, departure, sequenceNumber));
        exception.Message.ShouldContain("Arrival time must be less than or equal to departure time");
    }
    

    private static Station GetValidStation()
    {
        var city = GetValidCity();
        var geoCoordinates = new GeographicalCoordinates(0, 0);
        return Station.Create("Test Station", city, geoCoordinates);
    }

    private static City GetValidCity() => City.Create("Test City", Country.FR, new List<string> { "Fun Fact" });
}