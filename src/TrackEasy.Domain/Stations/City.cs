using FluentValidation;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Stations;

public sealed class City : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Country Country { get; private set; }

    public static City Create(string name, Country country)
    {
        var city = new City
        {
            Id = Guid.NewGuid(),
            Name = name,
            Country = country
        };

        new CityValidator().ValidateAndThrow(city);
        return city;
    }

    public void Update(string name, Country country)
    {
        Name = name;
        Country = country;

        new CityValidator().ValidateAndThrow(this);
    }

#pragma warning disable CS8618, CS9264
    private City() { }
#pragma warning restore CS8618, CS9264
}
