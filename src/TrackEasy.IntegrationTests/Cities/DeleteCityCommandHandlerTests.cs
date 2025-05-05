using Shouldly;
using TrackEasy.Application.Cities.CreateCity;
using TrackEasy.Application.Cities.DeleteCity;
using TrackEasy.Application.Cities.FindCity;
using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Cities;

public class DeleteCityCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task DeleteCityCommandHandler_ShouldDeleteCity_WhenCityExists()
    {
        // Arrange
        var command = new CreateCityCommand("City to delete", Country.AD, ["Fun fact 1", "Fun fact 2"]);
        var cityId = await Sender.Send(command);
        
        // Act
        await Sender.Send(new DeleteCityCommand(cityId));
        
        // Assert
        var city = await Sender.Send(new FindCityQuery(cityId));
        city.ShouldBeNull();
    }
    
    [Fact]
    public async Task DeleteCityCommandHandler_ShouldThrowException_WhenCityDoesNotExist()
    {
        // Arrange
        var nonExistentCityId = Guid.NewGuid();
        
        // Act
        var act = async () => await Sender.Send(new DeleteCityCommand(nonExistentCityId));
        
        // Assert
        await act.ShouldThrowAsync<TrackEasyException>();
    }
}