using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Users;

public sealed record UserCreatedEvent(Guid UserId) : IDomainEvent;
