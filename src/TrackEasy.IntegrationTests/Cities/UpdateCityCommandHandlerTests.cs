using Shouldly;
using TrackEasy.Application.Cities.CreateCity;
using TrackEasy.Application.Cities.FindCity;
using TrackEasy.Application.Cities.UpdateCity;
using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.IntegrationTests.Cities;

public class UpdateCityCommandHandlerTests(DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task UpdateCity_ValidDataProvided_ShouldUpdateCity()
    {
        // Arrange
        var createCommand = new CreateCityCommand("Old City", Country.AD, ["Fun fact 1", "Fun fact 2"]);
        var cityId = await Sender.Send(createCommand);

        var updateCommand = new UpdateCityCommand(cityId, "New City", Country.AD, ["Updated fun fact"]);

        // Act
        await Sender.Send(updateCommand);

        // Assert
        var updatedCity = await Sender.Send(new FindCityQuery(cityId));
        updatedCity.ShouldNotBeNull();
        updatedCity.Name.ShouldBe("New City");
        updatedCity.Country.Id.ShouldBe((int)Country.AD);
        updatedCity.FunFacts.ShouldContain("Updated fun fact");
    }
    
    [Fact]
    public async Task UpdateCity_InvalidIdProvided_ShouldThrowException()
    {
        // Arrange
        var updateCommand = new UpdateCityCommand(Guid.NewGuid(), "New City", Country.AD, ["Updated fun fact"]);

        // Act
        var act = async () => await Sender.Send(updateCommand);

        // Assert
        await act.ShouldThrowAsync<TrackEasyException>();
    }
    
    [Fact]
    public async Task UpdateCity_EmptyNameProvided_ShouldThrowException()
    {
        // Arrange
        var createCommand = new CreateCityCommand("Old City", Country.AD, ["Fun fact 1", "Fun fact 2"]);
        var cityId = await Sender.Send(createCommand);

        var updateCommand = new UpdateCityCommand(cityId, "", Country.AD, ["Updated fun fact"]);

        // Act
        var act = async () => await Sender.Send(updateCommand);

        // Assert
        await act.ShouldThrowAsync<FluentValidation.ValidationException>();
    }
}