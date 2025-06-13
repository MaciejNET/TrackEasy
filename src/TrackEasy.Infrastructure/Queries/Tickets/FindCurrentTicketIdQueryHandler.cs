using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Security;
using TrackEasy.Application.Tickets.FindCurrentTicketId;
using TrackEasy.Application.Tickets.GetTickets;
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
        
        var now = timeProvider.GetLocalNow();

        var ticketId = await dbContext.Tickets
            .AsNoTracking()
            .WithTicketType(TicketType.CURRENT, timeProvider)
            .WithUserId(userId.Value)
            .OrderBy(x => x.Stations.First(s => s.SequenceNumber == 1).DepartureTime)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        return ticketId;
    }
}