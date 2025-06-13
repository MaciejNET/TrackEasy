namespace TrackEasy.Application.Connections.GetConnectionChangeRequests;

using TrackEasy.Domain.Connections;

public sealed record ConnectionChangeRequestDto(
    Guid ConnectionId,
    string Name,
    string OperatorName,
    string StartStation,
    string EndStation,
    ConnectionRequestType RequestType);
