using FluentValidation;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

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
    
    public double CalculateDistanceTo(GeographicalCoordinates other)
    {
        const double EarthRadius = 6371; // Radius of the Earth in kilometers
        
        var dLat = (other.Latitude - Latitude) * (decimal)(Math.PI / 180);
        var dLon = (other.Longitude - Longitude) * (decimal)(Math.PI / 180);
        
        var a = Math.Sin((double)(dLat / 2)) * Math.Sin((double)(dLat / 2)) +
                Math.Cos((double)(Latitude * (decimal)(Math.PI / 180))) * Math.Cos((double)(other.Latitude * (decimal)(Math.PI / 180))) *
                Math.Sin((double)(dLon / 2)) * Math.Sin((double)(dLon / 2));
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return EarthRadius * c; // Distance in kilometers
    }
    
    private GeographicalCoordinates() {}
}