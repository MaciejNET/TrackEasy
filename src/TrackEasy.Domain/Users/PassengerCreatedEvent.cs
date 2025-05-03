using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Users;

public sealed record PassengerCreatedEvent(User User) : IDomainEvent;