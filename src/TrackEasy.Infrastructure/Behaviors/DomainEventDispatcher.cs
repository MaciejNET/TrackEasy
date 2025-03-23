using MediatR;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Infrastructure.Behaviors;

internal sealed class DomainEventDispatcher(TrackEasyDbContext dbContext, IMediator mediator)
{
    public async Task DispatchEventsAsync(CancellationToken cancellationToken)
    {
        var entities = dbContext.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .Select(x => x.Entity)
            .ToList();
        
        var domainEvents = entities
            .SelectMany(x => x.DomainEvents)
            .ToList();
        
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }
    }
}