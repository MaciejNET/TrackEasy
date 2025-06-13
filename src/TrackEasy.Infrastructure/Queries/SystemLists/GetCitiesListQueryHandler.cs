using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetCitiesListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetCitiesListQuery, IEnumerable<SystemListItemDto>>
{
    public async Task<IEnumerable<SystemListItemDto>> Handle(GetCitiesListQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Cities
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new SystemListItemDto(c.Id, c.Name))
            .ToListAsync(cancellationToken);
    }
}