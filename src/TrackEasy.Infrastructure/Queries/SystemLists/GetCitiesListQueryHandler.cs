using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetCitiesListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetCitiesListQuery, IEnumerable<SystemListItemDto>>
{
    public Task<IEnumerable<SystemListItemDto>> Handle(GetCitiesListQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}