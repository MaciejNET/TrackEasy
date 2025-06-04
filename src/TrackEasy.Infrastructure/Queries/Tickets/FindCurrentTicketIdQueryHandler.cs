using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Security;
using TrackEasy.Application.Tickets.FindCurrentTicketId;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Tickets;

internal sealed class FindCurrentTicketIdQueryHandler(TrackEasyDbContext dbContext, IUserContext userContext, TimeProvider timeProvider)
    : IQueryHandler<FindCurrentTicketIdQuery, Guid?>
{
    public async Task<Guid?> Handle(FindCurrentTicketIdQuery request, CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;
        
        if (userId is null)
        {
            return null;
        }
        
        var now = timeProvider.GetUtcNow();

        var ticketId = await dbContext.Tickets
            .AsNoTracking()
            .Where(x => x.ConnectionDate == DateOnly.FromDateTime(now.Date))
            .Where(x => x.Stations.Any(s =>
                s.SequenceNumber == 1 && s.DepartureTime >= TimeOnly.FromDateTime(now.DateTime)))
            .Where(x => x.Stations.Any(s =>
                s.SequenceNumber == x.Stations.Count && s.ArrivalTime <= TimeOnly.FromDateTime(now.DateTime)))
            .Where(x => x.PassengerId == userId)
            .OrderBy(x => x.Stations.First(s => s.SequenceNumber == 1).DepartureTime)
            .Select(x => x.Id)
            .SingleOrDefaultAsync(cancellationToken);
        
        return ticketId;
    }
}