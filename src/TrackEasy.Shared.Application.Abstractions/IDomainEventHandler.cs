using MediatR;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Shared.Application.Abstractions;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent;