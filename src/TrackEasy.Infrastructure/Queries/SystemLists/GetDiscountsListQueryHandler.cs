using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetDiscountsListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetDiscountsListQuery, IEnumerable<SystemListItemDto>>
{
    public async Task<IEnumerable<SystemListItemDto>> Handle(GetDiscountsListQuery request, CancellationToken cancellationToken)
    {
        var result = await dbContext.Discounts
            .AsNoTracking()
            .Select(d => new SystemListItemDto(d.Id, d.Name))
            .ToListAsync(cancellationToken);

        return result;
    }
}