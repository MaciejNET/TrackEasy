using FluentValidation;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Cities;

public sealed class City : AggregateRoot
{
    private List<string> _funFacts = [];
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Country Country { get; private set; }
    public IReadOnlyList<string> FunFacts => _funFacts.AsReadOnly();

    public static City Create(string name, Country country, IEnumerable<string> funFacts)
    {
        var city = new City
        {
            Id = Guid.NewGuid(),
            Name = name,
            Country = country,
            _funFacts = [..funFacts]
        };

        new CityValidator().ValidateAndThrow(city);
        return city;
    }

    public void Update(string name, Country country, IEnumerable<string> funFacts)
    {
        Name = name;
        Country = country;
        _funFacts = [..funFacts];

        new CityValidator().ValidateAndThrow(this);
    }

#pragma warning disable CS8618, CS9264
    private City() { }
#pragma warning restore CS8618, CS9264
}
