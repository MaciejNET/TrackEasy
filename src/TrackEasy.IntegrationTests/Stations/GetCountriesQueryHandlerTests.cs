using Shouldly;
using TrackEasy.Application.Cities.GetCountries;
using TrackEasy.Domain.Cities;

namespace TrackEasy.IntegrationTests.Stations;

public class GetCountriesQueryHandlerTests (DatabaseFixture databaseFixture) : IntegrationTestBase(databaseFixture)
{
    [Fact]
    public async Task GetCountriesQuery_ValidRequest_ShouldReturnCountries()
    {
        // Arrange
        var query = new GetCountriesQuery();

        // Act
        var countries = await Sender.Send(query);

        // Assert
        countries.Count().ShouldBe(Enum.GetValues<Country>().Length);
    }
}