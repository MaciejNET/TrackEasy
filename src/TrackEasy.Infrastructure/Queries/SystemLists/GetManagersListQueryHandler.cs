using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetManagersListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetManagersListQuery, IEnumerable<SystemListItemDto>>
{
    public async Task<IEnumerable<SystemListItemDto>> Handle(GetManagersListQuery request, CancellationToken cancellationToken)
    {
        var result = await dbContext.Managers
            .AsNoTracking()
            .Where(x => x.OperatorId == request.OperatorId)
            .OrderBy(m => m.User.FirstName)
            .ThenBy(m => m.User.LastName)
            .Select(m => new SystemListItemDto(
                m.User.Id,
                m.User.FirstName + " " + m.User.LastName
            ))
            .ToListAsync(cancellationToken);

        return result;
    }
}