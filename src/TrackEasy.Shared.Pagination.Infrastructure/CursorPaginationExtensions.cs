using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Shared.Pagination.Infrastructure;

public static class CursorPaginationExtensions
{
    public static async Task<PaginatedCursorResult<T>> PaginateCursorAsync<T, TKey>(
        this IQueryable<T> query,
        int pageSize,
        string? currentCursor,
        Expression<Func<T, TKey>> cursorSelector,
        Func<string, TKey> cursorParser,
        CancellationToken cancellationToken = default) where TKey : struct
    {
        ValidateCursorInput(pageSize, currentCursor);

        var cursorValue = ParseCursor(currentCursor, cursorParser);
        var orderedQuery = ApplyCursorFilter(query, cursorSelector, cursorValue);
        
        var items = await orderedQuery
            .Take(pageSize + 1)
            .ToListAsync(cancellationToken);

        return ProcessCursorResults(items, pageSize, cursorSelector);
    }

    private static TKey? ParseCursor<TKey>(string? currentCursor, Func<string, TKey> cursorParser) 
        where TKey : struct
    {
        if (string.IsNullOrEmpty(currentCursor)) return null;
        
        try
        {
            return cursorParser(currentCursor);
        }
        catch
        {
            throw new ArgumentException("Invalid cursor value", nameof(currentCursor));
        }
    }

    private static IQueryable<T> ApplyCursorFilter<T, TKey>(
        IQueryable<T> query,
        Expression<Func<T, TKey>> cursorSelector,
        TKey? cursorValue) where TKey : struct
    {
        if (!cursorValue.HasValue) return query.OrderBy(cursorSelector);

        var parameter = cursorSelector.Parameters[0];
        var memberAccess = cursorSelector.Body;
        var constant = Expression.Constant(cursorValue.Value, typeof(TKey));
        var comparison = Expression.GreaterThan(memberAccess, constant);
        var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);

        return query
            .OrderBy(cursorSelector)
            .Where(lambda);
    }

    private static PaginatedCursorResult<T> ProcessCursorResults<T, TKey>(
        List<T> items,
        int pageSize,
        Expression<Func<T, TKey>> cursorSelector) where TKey : struct
    {
        var hasNextPage = items.Count > pageSize;
        if (hasNextPage)
        {
            items = items.Take(pageSize).ToList();
        }

        var nextCursor = hasNextPage
            ? cursorSelector.Compile()(items.Last()).ToString()
            : null;

        return new PaginatedCursorResult<T>(items, nextCursor, hasNextPage);
    }

    private static void ValidateCursorInput(int pageSize, string? currentCursor)
    {
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(
                nameof(pageSize), 
                "Page size must be at least 1.");
    }
}