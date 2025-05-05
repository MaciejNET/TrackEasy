using Shouldly;
using TrackEasy.Domain.Stations;

namespace TrackEasy.UnitTests.Stations;

public class GeographicalCoordinatesTests
{
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(45, 90)]
    [InlineData(-90, -180)]
    [InlineData(90, 180)]
    public void Constructor_Should_CreateInstance_For_ValidCoordinates(int latitude, int longitude)
    {
        var coordinates = new GeographicalCoordinates(latitude, longitude);

        coordinates.Latitude.ShouldBe(latitude);
        coordinates.Longitude.ShouldBe(longitude);
    }

    [Theory]
    [InlineData(-91, 0)]
    [InlineData(91, 0)]
    [InlineData(0, -181)]
    [InlineData(0, 181)]
    [InlineData(-91, -181)]
    [InlineData(91, 181)]
    public void Constructor_Should_ThrowValidationException_For_InvalidCoordinates(int latitude, int longitude)
    {
        Should.Throw<FluentValidation.ValidationException>(() => new GeographicalCoordinates(latitude, longitude));
    }
}