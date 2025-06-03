namespace TrackEasy.Application.Tickets.GetTickets;

public sealed record TicketDto(
    Guid Id,
    string StartStation,
    string EndStation,
    TimeOnly DepartureTime,
    TimeOnly ArrivalTime,
    DateOnly ConnectionDate);