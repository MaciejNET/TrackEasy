using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Operators.FindCoach;
using TrackEasy.Infrastructure.Database;
using static TrackEasy.Infrastructure.Queries.Operators.Extensions;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Operators;

internal sealed class FindCoachQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<FindCoachQuery, CoachDetailsDto?>
{
    public async Task<CoachDetailsDto?> Handle(FindCoachQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Coaches
            .WithOperatorId(request.OperatorId)
            .WithCoachId(request.Id)
            .Select(c => new CoachDetailsDto(
                c.Id,
                c.Code,
                c.Seats.Select(s => s.Number)))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
