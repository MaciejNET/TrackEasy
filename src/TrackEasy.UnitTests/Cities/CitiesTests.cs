using FluentValidation;
using Shouldly;
using TrackEasy.Domain.Cities;

namespace TrackEasy.UnitTests.Cities;

public class CitiesTests
{
    private static City CreateValidCity() =>
        City.Create("Test City", Country.PL, new List<string> { "Has a river", "Old town" });

    [Fact]
    public void Create_Should_CreateCity_For_ValidData()
    {
        var city = CreateValidCity();

        city.Name.ShouldBe("Test City");
        city.Country.ShouldBe(Country.PL);
        city.FunFacts.Count.ShouldBe(2);
        city.Id.ShouldNotBe(Guid.Empty);
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    [InlineData(null)]
    public void Create_Should_ThrowValidationException_For_InvalidName(string name)
    {
        Should.Throw<ValidationException>(() =>
            City.Create(name, Country.PL, new List<string> { "Fact" }));
    }

    [Fact]
    public void Create_Should_ThrowValidationException_For_EmptyFunFacts()
    {
        Should.Throw<ValidationException>(() =>
            City.Create("Valid Name", Country.PL, new List<string> { string.Empty, string.Empty }));
    }

    [Fact]
    public void Create_Should_ThrowValidationException_For_ShortFunFact()
    {
        Should.Throw<ValidationException>(() =>
            City.Create("Valid Name", Country.PL, new List<string> { "Hi" }));
    }

    [Fact]
    public void Update_Should_UpdateNameCountryAndFunFacts()
    {
        var city = CreateValidCity();
        city.Update("New Name", Country.DE, new List<string> { "New fact" });

        city.Name.ShouldBe("New Name");
        city.Country.ShouldBe(Country.DE);
        city.FunFacts.Count.ShouldBe(1);
        city.FunFacts[0].ShouldBe("New fact");
    }

    [Fact]
    public void Update_Should_ThrowValidationException_For_InvalidName()
    {
        var city = CreateValidCity();

        Should.Throw<ValidationException>(() =>
            city.Update("", Country.PL, new List<string> { "Fact" }));
    }
}