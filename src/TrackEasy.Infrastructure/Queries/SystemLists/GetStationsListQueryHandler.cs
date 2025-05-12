using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace TrackEasy.Infrastructure.Queries.SystemLists
{
    internal sealed class GetStationsListQueryHandler : IQueryHandler<GetStationsListQuery, IEnumerable<SystemListItemDto>>
    {
        private readonly TrackEasyDbContext _dbContext;

        public GetStationsListQueryHandler(TrackEasyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SystemListItemDto>> Handle(GetStationsListQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Stations
                .AsNoTracking()
                .Select(s => new SystemListItemDto(s.Id, s.Name))
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}

