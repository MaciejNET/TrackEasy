using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Users;

public sealed record AdminCreatedEvent(User User) : IDomainEvent;