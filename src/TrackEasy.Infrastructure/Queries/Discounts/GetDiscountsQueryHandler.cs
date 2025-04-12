using Microsoft.EntityFrameworkCore;

using TrackEasy.Application.Discounts.GetDiscounts;
using TrackEasy.Application.Discounts.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Discounts;

internal sealed class GetDiscountsQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetDiscountsQuery, PaginatedResult<DiscountDto>>
{
    public async Task<PaginatedResult<DiscountDto>> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Discounts
            .AsNoTracking()
            .Select(x => new DiscountDto(x.Id, x.Name, x.Percentage))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}