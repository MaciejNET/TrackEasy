namespace TrackEasy.Application.Tickets.FindTicket;

public sealed record TicketConnectionStationDto(
    string Name,
    TimeOnly? ArrivalTime,
    TimeOnly? DepartureTime,
    int SequenceNumber);