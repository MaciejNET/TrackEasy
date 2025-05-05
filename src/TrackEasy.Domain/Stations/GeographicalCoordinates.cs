using FluentValidation;

namespace TrackEasy.Domain.Stations;

public sealed record GeographicalCoordinates
{
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    
    public GeographicalCoordinates(decimal latitude, decimal longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        
        new GeographicalCoordinatesValidator().ValidateAndThrow(this);
    }
}