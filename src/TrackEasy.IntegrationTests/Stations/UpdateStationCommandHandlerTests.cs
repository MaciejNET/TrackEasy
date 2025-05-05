using Shouldly;
using TrackEasy.Application.Cities.CreateCity;
using TrackEasy.Application.Stations;
using TrackEasy.Application.Stations.CreateStation;
using TrackEasy.Application.Stations.FindStation;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Application.Stations.UpdateStation;
using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Stations;

public class UpdateStationCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task UpdateStation_ValidData_ShouldUpdateStation()
    {
        var createCityCommand = new CreateCityCommand("Test City", Country.AD, ["Fun Fact"]);
        var cityId = await Sender.Send(createCityCommand);
        
        var createStationCommand = new CreateStationCommand("Test", cityId, new GeographicalCoordinatesDto(62.2M, 48.8M));
        var stationId = await Sender.Send(createStationCommand);

        var command = new UpdateStationCommand(stationId, "Updated Test", new GeographicalCoordinatesDto(63.3M, 49.9M));
        await Sender.Send(command);

        var station = await Sender.Send(new FindStationQuery(stationId));
        station.ShouldNotBeNull();
        station!.Name.ShouldBe("Updated Test");
        station.GeographicalCoordinates.Latitude.ShouldBe(63.3M);
        station.GeographicalCoordinates.Longitude.ShouldBe(49.9M);
    }
    
    [Fact]
    public async Task UpdateStation_InvalidData_ShouldThrowException()
    {
        var createCityCommand = new CreateCityCommand("Test City", Country.AD, ["Fun Fact"]);
        var cityId = await Sender.Send(createCityCommand);
        
        var createStationCommand = new CreateStationCommand("Test", cityId, new GeographicalCoordinatesDto(62.2M, 48.8M));
        var stationId = await Sender.Send(createStationCommand);

        var command = new UpdateStationCommand(stationId, "", new GeographicalCoordinatesDto(200M, 48.8M));

        await Should.ThrowAsync<FluentValidation.ValidationException>(async () => await Sender.Send(command));
    }
    
    [Fact]
    public async Task UpdateStation_StationNotFound_ShouldThrowException()
    {
        var command = new UpdateStationCommand(Guid.NewGuid(), "Updated Test", new GeographicalCoordinatesDto(63.3M, 49.9M));

        var exception = await Should.ThrowAsync<TrackEasyException>(async () => await Sender.Send(command));
        exception.Code.ShouldBe(SharedCodes.EntityNotFound);
    }
    
    [Fact]
    public async Task UpdateStation_StationAlreadyExists_ShouldThrowException()
    {
        var createCityCommand = new CreateCityCommand("Test City", Country.AD, ["Fun Fact"]);
        var cityId = await Sender.Send(createCityCommand);
        
        var createStationCommand = new CreateStationCommand("Test", cityId, new GeographicalCoordinatesDto(62.2M, 48.8M));
        var stationId = await Sender.Send(createStationCommand);

        var command = new CreateStationCommand("Test 2", cityId, new GeographicalCoordinatesDto(63.3M, 49.9M));
        await Sender.Send(command);

        var updateCommand = new UpdateStationCommand(stationId, "Test 2", new GeographicalCoordinatesDto(63.3M, 49.9M));

        var exception = await Should.ThrowAsync<TrackEasyException>(async () => await Sender.Send(updateCommand));
        exception.Code.ShouldBe(Codes.StationAlreadyExists);
    }
}