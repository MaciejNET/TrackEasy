using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetStationListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetStationListQuery, IEnumerable<SystemListItemDto>>
{
    public Task<IEnumerable<SystemListItemDto>> Handle(GetStationListQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}