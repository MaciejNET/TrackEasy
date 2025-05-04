using TrackEasy.Domain.Discounts;

namespace TrackEasy.Infrastructure.Queries.Discounts;

public static class Extensions
{
    public static IQueryable<Discount> WithDiscountId(this IQueryable<Discount> queryable, Guid? discountId)
    {
        if (discountId == null || discountId == Guid.Empty)
            return queryable;

        return queryable.Where(d => d.Id == discountId);
    }
    
    public static IQueryable<Discount> WithName(this IQueryable<Discount> queryable, string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return queryable;

        return queryable.Where(d => d.Name.Contains(name));
    }
    
    public static IQueryable<Discount> WithPercentage(this IQueryable<Discount> queryable, decimal? percentage)
    {
        if (percentage == null || percentage == 0)
            return queryable;

        return queryable.Where(d => d.Percentage == percentage);
    }
}