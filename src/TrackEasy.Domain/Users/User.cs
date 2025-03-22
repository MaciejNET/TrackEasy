using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Users;

public sealed class User : IdentityUser<Guid>, IAggregateRoot
{
    [PersonalData]
    public string? FirstName { get; private set; }
    
    [PersonalData]
    public string? LastName { get; private set; }
    
    [PersonalData]
    public DateOnly? DateOfBirth { get; private set; }
    
    public void UpdatePersonalData(string firstName, string lastName, DateOnly dateOfBirth, TimeProvider timeProvider)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        
        new UserValidator(timeProvider).ValidateAndThrow(this);
    }

#pragma warning disable CS8618, CS9264
    public User() { }
#pragma warning restore CS8618, CS9264
    
    // Implicit implementation of IAggregateRoot due to use of IdentityUser<Guid>
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}