using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Connections;

public sealed record ConnectionRequestCreatedEvent(Guid ConnectionId, string Name, string OperatorName, ConnectionRequestType Type) : IDomainEvent;