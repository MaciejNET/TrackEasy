using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Users;

public sealed record UserCreatedEvent(User User) : IDomainEvent;