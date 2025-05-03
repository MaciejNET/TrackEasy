using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Users;

public sealed record ManagerCreatedEvent(User User) : IDomainEvent;