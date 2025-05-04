using TrackEasy.Domain.Stations;

namespace TrackEasy.Infrastructure.Queries.Stations;

public static class Extensions
{
    public static IQueryable<Station> WithStationName(this IQueryable<Station> queryable, string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return queryable;

        return queryable.Where(station => station.Name.Contains(name));
    }
    
    public static IQueryable<Station> WithCityName(this IQueryable<Station> queryable, string? cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return queryable;

        return queryable.Where(station => station.City.Name.Contains(cityName));
    }
}