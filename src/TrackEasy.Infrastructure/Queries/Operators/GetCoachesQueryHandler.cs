using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Operators;

internal sealed class GetCoachesQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetCoachesQuery, PaginatedResult<CoachDto>>
{
    public Task<PaginatedResult<CoachDto>> Handle(GetCoachesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}