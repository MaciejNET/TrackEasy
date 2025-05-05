using Shouldly;
using TrackEasy.Application.Cities.CreateCity;
using TrackEasy.Application.Stations.CreateStation;
using TrackEasy.Application.Stations.DeleteStation;
using TrackEasy.Application.Stations.FindStation;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Stations;

public class DeleteStationCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task DeleteStation_ValidId_ShouldDeleteStation()
    {
        var createCityCommand = new CreateCityCommand("Test City", Country.AD, ["Fun Fact"]);
        var cityId = await Sender.Send(createCityCommand);
        
        var createStationCommand = new CreateStationCommand("Test", cityId, new GeographicalCoordinatesDto(62.2M, 48.8M));
        var stationId = await Sender.Send(createStationCommand);

        await Sender.Send(new DeleteStationCommand(stationId));

        var station = await Sender.Send(new FindStationQuery(stationId));
        station.ShouldBeNull();
    }
    
    [Fact]
    public async Task DeleteStation_InvalidId_ShouldThrowException()
    {
        var command = new DeleteStationCommand(Guid.NewGuid());

        var exception = await Should.ThrowAsync<TrackEasyException>(async () => await Sender.Send(command));
        exception.Code.ShouldBe(SharedCodes.EntityNotFound);
    }
}