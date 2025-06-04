namespace TrackEasy.Application.Connections.Shared;

public sealed record ConnectionStationDto(Guid StationId, TimeOnly? ArrivalTime, TimeOnly? DepartureTime, int SequenceNumber);