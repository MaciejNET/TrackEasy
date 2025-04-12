using System;

namespace TrackEasy.Domain.Stations;

public class City
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CityEnum CityEnum { get; set; }
}
public enum CityEnum
{
    Kielce,
    Krakow
}