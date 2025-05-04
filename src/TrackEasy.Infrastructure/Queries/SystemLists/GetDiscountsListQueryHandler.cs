using TrackEasy.Application.SystemLists;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetDiscountsListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetDiscountsListQuery, IEnumerable<SystemListItemDto>>
{
    public Task<IEnumerable<SystemListItemDto>> Handle(GetDiscountsListQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}