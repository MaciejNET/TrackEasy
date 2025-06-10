using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.FindConnectionChangeRequest;

public sealed record FindConnectionChangeRequestQuery(Guid ConnectionId)
    : IQuery<ConnectionChangeRequestDetailsDto?>;
