using Shouldly;
using TrackEasy.Domain.Cities;
using TrackEasy.Domain.Stations;

namespace TrackEasy.UnitTests.Stations;

public class StationTests
{
    private static City CreateValidCity() => 
        City.Create("Test City", Country.AD, new List<string> { "Fun fact 1" });

    private static GeographicalCoordinates CreateValidCoordinates() => new(10, 20);

    [Fact]
    public void Create_Should_CreateStation_For_ValidData()
    {
        var city = CreateValidCity();
        var coordinates = CreateValidCoordinates();

        var station = Station.Create("Central Station", city, coordinates);

        station.Name.ShouldBe("Central Station");
        station.City.ShouldBe(city);
        station.GeographicalCoordinates.ShouldBe(coordinates);
        station.Id.ShouldNotBe(Guid.Empty);
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    public void Create_Should_ThrowValidationException_For_InvalidName(string name)
    {
        var city = CreateValidCity();
        var coordinates = CreateValidCoordinates();

        Should.Throw<FluentValidation.ValidationException>(() =>
            Station.Create(name, city, coordinates));
    }
    
    [Fact]
    public void Update_Should_UpdateNameAndCoordinates()
    {
        var city = CreateValidCity();
        var coordinates = CreateValidCoordinates();
        var station = Station.Create("Old Name", city, coordinates);

        var newCoordinates = new GeographicalCoordinates(30, 40);
        station.Update("New Name", newCoordinates);

        station.Name.ShouldBe("New Name");
        station.GeographicalCoordinates.ShouldBe(newCoordinates);
    }

    [Fact]
    public void Update_Should_ThrowValidationException_For_InvalidName()
    {
        var city = CreateValidCity();
        var coordinates = CreateValidCoordinates();
        var station = Station.Create("Valid Name", city, coordinates);

        Should.Throw<FluentValidation.ValidationException>(() =>
            station.Update("", coordinates));
    }
}