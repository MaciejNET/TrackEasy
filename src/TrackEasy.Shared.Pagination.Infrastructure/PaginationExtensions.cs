using Microsoft.EntityFrameworkCore;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Shared.Pagination.Infrastructure;

public static class PaginationExtensions
{
    public static async Task<PaginatedResult<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        ValidateInput(pageNumber, pageSize);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await GetPageItems(query, pageNumber, pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items, pageNumber, pageSize, totalCount);
    }
    
    private static IQueryable<T> GetPageItems<T>(IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
    
    private static void ValidateInput(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(
                nameof(pageNumber), 
                "Page number must be at least 1.");
                
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(
                nameof(pageSize), 
                "Page size must be at least 1.");
    }
}