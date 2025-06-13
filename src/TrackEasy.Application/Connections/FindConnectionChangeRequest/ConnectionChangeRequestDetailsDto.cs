using TrackEasy.Application.Connections.Shared;
using TrackEasy.Domain.Connections;

namespace TrackEasy.Application.Connections.FindConnectionChangeRequest;

public sealed record ConnectionChangeRequestDetailsDto(
    Guid ConnectionId,
    string Name,
    string OperatorName,
    ConnectionRequestType RequestType,
    ScheduleDto? Schedule,
    IEnumerable<ConnectionStationDto>? Stations);
