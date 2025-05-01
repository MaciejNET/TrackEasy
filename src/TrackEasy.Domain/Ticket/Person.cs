using FluentValidation;

namespace TrackEasy.Domain.Ticket;

public sealed record Person
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public string? Discount { get; private set; }
    
    public Person(string firstName, string lastName, DateOnly dateOfBirth, string? discount)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Discount = discount;
        new PersonValidator().ValidateAndThrow(this);
    }
}