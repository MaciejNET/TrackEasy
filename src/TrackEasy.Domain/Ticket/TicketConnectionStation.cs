namespace TrackEasy.Domain.Ticket;

public sealed record TicketConnectionStation(
    string Name,
    TimeOnly? ArrivalTime,
    TimeOnly? DepartureTime,
    int SequenceNumber);