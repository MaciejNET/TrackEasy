using Shouldly;
using TrackEasy.Application.Cities.CreateCity;
using TrackEasy.Application.Connections.CreateConnection;
using TrackEasy.Application.Connections.FindConnection;
using TrackEasy.Application.Connections.Shared;
using TrackEasy.Application.Shared;
using TrackEasy.Application.Connections.UpdateConnection;
using TrackEasy.Application.Operators.AddCoach;
using TrackEasy.Application.Operators.AddTrain;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Domain.Cities;
using TrackEasy.Domain.Shared;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Connections;

public class UpdateConnectionCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    private async Task<Guid> PrepareConnection()
    {
        var city1 = await Sender.Send(new CreateCityCommand("City1", Country.AD, []));
        var city2 = await Sender.Send(new CreateCityCommand("City2", Country.AD, []));
        var station1 = await Sender.Send(new TrackEasy.Application.Stations.CreateStation.CreateStationCommand("Station1", city1, new TrackEasy.Application.Stations.Shared.GeographicalCoordinatesDto(1,1)));
        var station2 = await Sender.Send(new TrackEasy.Application.Stations.CreateStation.CreateStationCommand("Station2", city2, new TrackEasy.Application.Stations.Shared.GeographicalCoordinatesDto(2,2)));

        var operatorId = await Sender.Send(new CreateOperatorCommand("Operator", "OP"));
        await Sender.Send(new AddCoachCommand(operatorId, "C1", [1,2]));
        var coach = (await Sender.Send(new GetCoachesQuery(operatorId, null, 1,10))).Items.First();
        var trainId = await Sender.Send(new AddTrainCommand(operatorId, "T1", [(coach.Id,1)]));

        var schedule = new ScheduleDto(new DateOnly(2025,1,1), new DateOnly(2025,12,31), [DayOfWeek.Monday]);
        var stations = new List<ConnectionStationDto>
        {
            new ConnectionStationDto(station1, null, new TimeOnly(8,0), 1),
            new ConnectionStationDto(station2, new TimeOnly(10,0), null, 2)
        };
        var command = new CreateConnectionCommand("Conn", operatorId, new MoneyDto(1, Currency.EUR), trainId, schedule, stations, true);
        return await Sender.Send(command);
    }

    [Fact]
    public async Task UpdateConnection_ValidData_ShouldUpdateConnection()
    {
        var id = await PrepareConnection();

        var command = new UpdateConnectionCommand(id, "Updated", new MoneyDto(2, Currency.EUR));
        await Sender.Send(command);

        var connection = await Sender.Send(new FindConnectionQuery(id));
        connection!.Name.ShouldBe("Updated");
        connection.PricePerKilometer.Amount.ShouldBe(2);
        connection.PricePerKilometer.Currency.ShouldBe(Currency.EUR);
    }

    [Fact]
    public async Task UpdateConnection_NotFound_ShouldThrowException()
    {
        var command = new UpdateConnectionCommand(Guid.NewGuid(), "Updated", new MoneyDto(2, Currency.EUR));
        await Should.ThrowAsync<TrackEasyException>(async () => await Sender.Send(command));
    }
}
