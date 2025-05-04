using TrackEasy.Domain.Cities;

namespace TrackEasy.Infrastructure.Queries.Cities;

public static class Extensions
{
    public static IQueryable<City> WithCityId(this IQueryable<City> queryable, Guid? cityId)
    {
        if (cityId == null || cityId == Guid.Empty)
            return queryable;

        return queryable.Where(c => c.Id == cityId);
    }
    
    public static IQueryable<City> WithName(this IQueryable<City> queryable, string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return queryable;

        return queryable.Where(c => c.Name.Contains(name));
    }
}