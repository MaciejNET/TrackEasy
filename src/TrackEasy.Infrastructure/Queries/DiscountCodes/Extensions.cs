using TrackEasy.Domain.DiscountCodes;

namespace TrackEasy.Infrastructure.Queries.DiscountCodes;

public static class Extensions
{
    public static IQueryable<DiscountCode> WithDiscountCodeId(this IQueryable<DiscountCode> queryable, Guid? discountCodeId)
    {
        if (discountCodeId == null || discountCodeId == Guid.Empty)
            return queryable;

        return queryable.Where(d => d.Id == discountCodeId);
    }
    
    public static IQueryable<DiscountCode> WithCode(this IQueryable<DiscountCode> queryable, string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return queryable;

        return queryable.Where(d => d.Code.Contains(code));
    }
    
    public static IQueryable<DiscountCode> WithExactCode(this IQueryable<DiscountCode> queryable, string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return queryable;

        return queryable.Where(d => d.Code == code);
    }
    
    public static IQueryable<DiscountCode> WithPercentage(this IQueryable<DiscountCode> queryable, decimal? percentage)
    {
        if (percentage is null or 0)
            return queryable;

        return queryable.Where(d => d.Percentage == percentage);
    }
}