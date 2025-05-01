using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Connections;

public sealed record ConnectionDeletedEvent(Guid ConnectionId) : IDomainEvent;