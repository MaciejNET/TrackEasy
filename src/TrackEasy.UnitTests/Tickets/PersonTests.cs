using FluentValidation;
using Shouldly;
using TrackEasy.Domain.Tickets;

namespace TrackEasy.UnitTests.Tickets;

public class PersonTests
{
    [Fact]
    public void CreatePerson_WithValidData_ShouldSucceed()
    {
        var person = new Person("Alice", "Smith", new DateOnly(1990, 5, 15), "DISC10");
        person.FirstName.ShouldBe("Alice");
        person.LastName.ShouldBe("Smith");
    }

    [Fact]
    public void CreatePerson_WithInvalidFirstName_ShouldThrowValidationException()
    {
        Should.Throw<ValidationException>(() =>
            new Person("", "Doe", new DateOnly(2000, 1, 1), null));
    }

    [Fact]
    public void CreatePerson_WithInvalidLastName_ShouldThrowValidationException()
    {
        Should.Throw<ValidationException>(() =>
            new Person("John", "", new DateOnly(2000, 1, 1), null));
    }

    [Fact]
    public void CreatePerson_WithInvalidDiscount_ShouldThrowValidationException()
    {
        // Discount must be at least 3 characters when provided.
        Should.Throw<ValidationException>(() =>
            new Person("John", "Doe", new DateOnly(2000, 1, 1), "12"));
    }
}