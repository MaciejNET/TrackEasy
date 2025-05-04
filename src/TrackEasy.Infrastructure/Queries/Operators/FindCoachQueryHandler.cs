using TrackEasy.Application.Operators.FindCoach;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Operators;

internal sealed class FindCoachQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<FindCoachQuery, CoachDetailsDto?>
{
    public Task<CoachDetailsDto?> Handle(FindCoachQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}