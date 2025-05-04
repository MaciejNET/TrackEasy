using FluentValidation;
using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Stations;

public sealed class Station : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid CityId { get; private set; }
    public City City { get; private set; }
    public GeographicalCoordinates GeographicalCoordinates { get; private set; }
    
    public static Station Create(string name, City city, GeographicalCoordinates geographicalCoordinates)
    {
        var station = new Station
        {
            Id = Guid.NewGuid(),
            Name = name,
            CityId = city.Id,
            City = city,
            GeographicalCoordinates = geographicalCoordinates
        };

        new StationValidator().ValidateAndThrow(station);
        return station;
    }
    
    public void Update(string name, GeographicalCoordinates geographicalCoordinates)
    {
        Name = name;
        GeographicalCoordinates = geographicalCoordinates;

        new StationValidator().ValidateAndThrow(this);
    }
    
#pragma warning disable CS8618, CS9264
    private Station() { }
#pragma warning restore CS8618, CS9264
}