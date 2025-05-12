using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace TrackEasy.Infrastructure.Queries.SystemLists
{
    internal sealed class
        GetOperatorsListQueryHandler : IQueryHandler<GetOperatorsListQuery, IEnumerable<SystemListItemDto>>
    {
        private readonly TrackEasyDbContext _dbContext;

        public GetOperatorsListQueryHandler(TrackEasyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SystemListItemDto>> Handle(GetOperatorsListQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.Operators
                .AsNoTracking()
                .Select(o => new SystemListItemDto(o.Id, o.Name))
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
