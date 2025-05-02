using FluentValidation;

namespace TrackEasy.Domain.Stations;

public sealed record GeographicalCoordinates
{
    public int Latitude { get; private set; }
    public int Longitude { get; private set; }
    
    public GeographicalCoordinates(int latitude, int longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
        
        new GeographicalCoordinatesValidator().ValidateAndThrow(this);
    }
}