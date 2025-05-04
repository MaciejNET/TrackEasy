using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetManagersListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetManagersListQuery, IEnumerable<SystemListItemDto>>
{
    public Task<IEnumerable<SystemListItemDto>> Handle(GetManagersListQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}