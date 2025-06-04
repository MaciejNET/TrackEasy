namespace TrackEasy.Application.Tickets.GetTicketCities;

public sealed record TicketCityDto(Guid Id, string Name, int SequenceNumber, bool IsLocked);