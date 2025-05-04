using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.DiscountCodes.GetDiscountCodes;
using TrackEasy.Application.DiscountCodes.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.DiscountCodes;

internal sealed class GetDiscountCodesQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetDiscountCodesQuery, PaginatedResult<DiscountCodeDto>>
{
    public async Task<PaginatedResult<DiscountCodeDto>> Handle(GetDiscountCodesQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.DiscountCodes
            .AsNoTracking()
            .WithCode(request.Code)
            .WithPercentage(request.Percentage)
            .Select(x => new DiscountCodeDto(x.Id, x.Code, x.Percentage, x.From, x.To))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
} 