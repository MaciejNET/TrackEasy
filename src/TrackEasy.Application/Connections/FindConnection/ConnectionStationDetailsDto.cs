namespace TrackEasy.Application.Connections.FindConnection;

public sealed record ConnectionStationDetailsDto(
    Guid Id,
    Guid StationId,
    string StationName,
    TimeOnly? ArrivalTime,
    TimeOnly? DepartureTime,
    int SequenceNumber
);