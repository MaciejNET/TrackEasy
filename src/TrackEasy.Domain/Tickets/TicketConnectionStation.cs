namespace TrackEasy.Domain.Tickets;

public sealed record TicketConnectionStation(
    string Name,
    TimeOnly? ArrivalTime,
    TimeOnly? DepartureTime,
    int SequenceNumber);