using Shouldly;
using TrackEasy.Application.Cities.CreateCity;
using TrackEasy.Application.Cities.FindCity;
using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Cities;

public class CreateCityCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task CreateCityCommandHandler_ShouldCreateCity_WhenCityDoesNotExist()
    {
        // Arrange
        var command = new CreateCityCommand("New City", Country.AD, ["Fun fact 1", "Fun fact 2"]);

        // Act
        var id = await Sender.Send(command);
        
        // Assert
        var city = await Sender.Send(new FindCityQuery(id));
        city.ShouldNotBeNull();
        city.Name.ShouldBe("New City");
        city.Country.Id.ShouldBe((int)Country.AD);
        city.FunFacts.ShouldNotBeEmpty();
    }
    
    [Fact]
    public async Task CreateCityCommandHandler_ShouldThrowException_WhenCityAlreadyExists()
    {
        // Arrange
        var command = new CreateCityCommand("Existing City", Country.AD, ["Fun fact 1", "Fun fact 2"]);
        await Sender.Send(command);

        // Act & Assert
        await Should.ThrowAsync<TrackEasyException>(async () => await Sender.Send(command));
    }
    
    [Fact]
    public async Task CreateCityCommandHandler_ShouldThrowException_WhenCityNameIsEmpty()
    {
        // Arrange
        var command = new CreateCityCommand("", Country.AD, ["Fun fact 1", "Fun fact 2"]);

        // Act & Assert
        await Should.ThrowAsync<FluentValidation.ValidationException>(async () => await Sender.Send(command));
    }
}