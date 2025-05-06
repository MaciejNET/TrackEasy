using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetStationsListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetStationsListQuery, IEnumerable<SystemListItemDto>>
{
    public Task<IEnumerable<SystemListItemDto>> Handle(GetStationsListQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}