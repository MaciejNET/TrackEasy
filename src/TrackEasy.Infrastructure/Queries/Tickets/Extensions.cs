using TrackEasy.Application.Tickets.GetTickets;
using TrackEasy.Domain.Tickets;

namespace TrackEasy.Infrastructure.Queries.Tickets;

public static class Extensions
{
    public static IQueryable<Ticket> WithTicketType(this IQueryable<Ticket> query, TicketType type,
        TimeProvider timeProvider)
    {
        var currentDate = DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime);
        var currentTime = TimeOnly.FromDateTime(timeProvider.GetUtcNow().DateTime);

        return type switch
        {
            TicketType.CURRENT => query.Where(x =>
                x.ConnectionDate > currentDate
                || (x.ConnectionDate == currentDate && x.Stations.Any(s => s.DepartureTime >= currentTime))),
            TicketType.ARCHIVED => query.Where(x => x.ConnectionDate < currentDate ||
                                                    (x.ConnectionDate == currentDate &&
                                                     x.Stations.Any(s => s.DepartureTime < currentTime))),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    public static IQueryable<Ticket> WithUserId(this IQueryable<Ticket> query, Guid userId)
    {
        return query.Where(x => x.PassengerId == userId);
    }
}