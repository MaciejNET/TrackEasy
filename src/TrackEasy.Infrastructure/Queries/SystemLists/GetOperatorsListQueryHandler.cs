using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetOperatorsListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetOperatorsListQuery, IEnumerable<SystemListItemDto>>
{
    public Task<IEnumerable<SystemListItemDto>> Handle(GetOperatorsListQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}