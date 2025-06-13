using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.SystemLists
{
    internal sealed class GetOperatorsListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetOperatorsListQuery, IEnumerable<SystemListItemDto>>
    {
        public async Task<IEnumerable<SystemListItemDto>> Handle(GetOperatorsListQuery request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Operators
                .AsNoTracking()
                .OrderBy(o => o.Name)
                .Select(o => new SystemListItemDto(o.Id, o.Name))
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
