using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Connections;

public sealed record ConnectionRequestApprovedEvent(Guid ConnectionId, string Name, Guid OperatorId, ConnectionRequestType RequestType) : IDomainEvent;