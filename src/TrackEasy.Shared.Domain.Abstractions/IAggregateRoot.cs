namespace TrackEasy.Shared.Domain.Abstractions;

public interface IAggregateRoot
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    protected void AddDomainEvent(IDomainEvent domainEvent);
    protected void ClearDomainEvents();
}