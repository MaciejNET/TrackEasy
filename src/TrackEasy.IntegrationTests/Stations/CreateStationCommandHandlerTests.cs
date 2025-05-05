using Shouldly;
using TrackEasy.Application.Cities.CreateCity;
using TrackEasy.Application.Stations;
using TrackEasy.Application.Stations.CreateStation;
using TrackEasy.Application.Stations.FindStation;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Stations;

public class CreateStationCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task CreateStation_ValidData_ShouldCreateStation()
    {
        var createCityCommand = new CreateCityCommand("Test City", Country.AD, ["Fun Fact"]);
        var cityId = await Sender.Send(createCityCommand);
        
        var command = new CreateStationCommand("Test", cityId, new GeographicalCoordinatesDto(62.2M, 48.8M));
        var stationId = await Sender.Send(command);

        var station = await Sender.Send(new FindStationQuery(stationId));
        station.ShouldNotBeNull();
        station!.Name.ShouldBe("Test");
        station.City.ShouldBe("Test City");
        station.GeographicalCoordinates.Latitude.ShouldBe(62.2M);
        station.GeographicalCoordinates.Longitude.ShouldBe(48.8M);
    }

    [Fact]
    public async Task CreateStation_InvalidCityId_ShouldThrowException()
    {
        var command = new CreateStationCommand("Test", Guid.NewGuid(), new GeographicalCoordinatesDto(62.2M, 48.8M));

        var exception = await Should.ThrowAsync<TrackEasyException>(async () => await Sender.Send(command));
        
        exception.Code.ShouldBe(SharedCodes.EntityNotFound);
    }
    
    [Fact]
    public async Task CreateStation_StationAlreadyExists_ShouldThrowException()
    {
        var createCityCommand = new CreateCityCommand("Test City", Country.AD, ["Fun Fact"]);
        var cityId = await Sender.Send(createCityCommand);
        
        var command = new CreateStationCommand("Test", cityId, new GeographicalCoordinatesDto(62.2M, 48.8M));
        await Sender.Send(command);

        var exception = await Should.ThrowAsync<TrackEasyException>(async () => await Sender.Send(command));
        exception.Code.ShouldBe(Codes.StationAlreadyExists);
    }
    
    [Fact]
    public async Task CreateStation_InvalidData_ShouldThrowException()
    {
        var createCityCommand = new CreateCityCommand("Test City", Country.AD, ["Fun Fact"]);
        var cityId = await Sender.Send(createCityCommand);
        
        var command = new CreateStationCommand("", cityId, new GeographicalCoordinatesDto(200M, 48.8M));

        await Should.ThrowAsync<FluentValidation.ValidationException>(async () => await Sender.Send(command));
    }
}