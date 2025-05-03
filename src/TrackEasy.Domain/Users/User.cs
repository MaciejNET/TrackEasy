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
    
    public static User CreatePassenger(
        string firstName,
        string lastName,
        string email,
        DateOnly dateOfBirth,
        TimeProvider timeProvider)
    {
        var user = CreateUser(firstName, lastName, email, dateOfBirth, false, timeProvider);
        user.AddDomainEvent(new PassengerCreatedEvent(user));
        return user;
    }

    public static User CreateManager(
        string firstName,
        string lastName,
        string email,
        DateOnly dateOfBirth,
        TimeProvider timeProvider)
    {
        var user = CreateUser(firstName, lastName, email, dateOfBirth, true, timeProvider);
        user.AddDomainEvent(new ManagerCreatedEvent(user));
        return user;
    }

    public static User CreateAdmin(
        string firstName,
        string lastName,
        string email,
        DateOnly dateOfBirth,
        TimeProvider timeProvider)
    {
        var user = CreateUser(firstName, lastName, email, dateOfBirth, true, timeProvider);
        user.AddDomainEvent(new AdminCreatedEvent(user));
        return user;
    }

    private static User CreateUser(
        string firstName,
        string lastName,
        string email,
        DateOnly dateOfBirth,
        bool twoFactorEnabled,
        TimeProvider timeProvider)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            UserName = email,
            Email = email,
            DateOfBirth = dateOfBirth,
            TwoFactorEnabled = twoFactorEnabled
        };

        new UserValidator(timeProvider).ValidateAndThrow(user);
        return user;
    }
    
    public void UpdatePersonalData(string firstName, string lastName, DateOnly dateOfBirth, TimeProvider timeProvider)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        
        new UserValidator(timeProvider).ValidateAndThrow(this);
    }
    
    // Implicit implementation of IAggregateRoot due to use of IdentityUser<Guid>
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}