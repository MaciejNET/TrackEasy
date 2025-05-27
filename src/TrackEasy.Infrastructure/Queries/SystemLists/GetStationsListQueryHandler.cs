using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.SystemLists
{
    internal sealed class GetStationsListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetStationsListQuery, IEnumerable<SystemListItemDto>>
    {
        public async Task<IEnumerable<SystemListItemDto>> Handle(GetStationsListQuery request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Stations
                .AsNoTracking()
                .Select(s => new SystemListItemDto(s.Id, s.Name))
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}

