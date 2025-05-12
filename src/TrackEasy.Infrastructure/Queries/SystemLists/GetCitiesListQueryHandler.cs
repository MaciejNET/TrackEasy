using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using Microsoft.EntityFrameworkCore; 

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetCitiesListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetCitiesListQuery, IEnumerable<SystemListItemDto>>
{
    public Task<IEnumerable<SystemListItemDto>> Handle(GetCitiesListQuery request, CancellationToken cancellationToken)
    {
        return dbContext.Cities
            .AsNoTracking()
            .Select(c => new SystemListItemDto(c.Id, c.Name))
            .ToListAsync(cancellationToken)
            .ContinueWith(t => t.Result.AsEnumerable(), cancellationToken);
    }
}