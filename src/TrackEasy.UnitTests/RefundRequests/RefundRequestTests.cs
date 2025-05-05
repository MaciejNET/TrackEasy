using FluentValidation;
using Microsoft.Extensions.Time.Testing;
using Shouldly;
using TrackEasy.Domain.RefundRequests;
using TrackEasy.Domain.Shared;
using TrackEasy.Domain.Tickets;

// Assuming Currency is here

namespace TrackEasy.UnitTests.RefundRequests;

public class RefundRequestTests
{
    private readonly FakeTimeProvider _fakeTimeProvider;

    public RefundRequestTests()
    {
        _fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 1, 10, 0, 0, TimeSpan.Zero));
    }

    // Helper methods to create valid Ticket components (similar to TicketTests)
    private static List<TicketConnectionStation> CreateValidStations() =>
    [
        new("Station A", null, new TimeOnly(10, 0), 1),
        new("Station B", new TimeOnly(11, 0), null, 2)
    ];

    private static List<Person> CreateValidPassengers(int count = 1) =>
        Enumerable.Range(1, count)
            .Select(i => new Person($"First{i}", $"Last{i}", new DateOnly(1990 + i, 1, 1), null))
            .ToList();

    private static Money CreateValidPrice() => new(100, Currency.USD); // Assuming Currency.USD

    // Helper to create a valid, real Ticket instance
    private Ticket CreateValidTicket(Guid? passengerId = null, List<int>? seatNumbers = null, Guid? operatorId = null)
    {
        return Ticket.Create(
            Guid.NewGuid(),
            "Express 123",
            CreateValidStations(),
            CreateValidPassengers(seatNumbers?.Count ?? 1),
            seatNumbers ?? [1],
            CreateValidPrice(),
            operatorId ?? Guid.NewGuid(),
            "OP-CODE",
            "Operator Name",
            passengerId,
            _fakeTimeProvider // Use the fake provider for creation
        );
    }


    [Fact]
    public void Create_Should_CreateRefundRequest_For_ValidDataWithUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var validTicket = CreateValidTicket(); // Create a real ticket
        const string reason = "Changed my travel plans";
        var expectedCreationTime = _fakeTimeProvider.GetUtcNow();

        // Act
        var refundRequest = RefundRequest.Create(userId, validTicket, reason, _fakeTimeProvider);

        // Assert
        refundRequest.ShouldNotBeNull();
        refundRequest.Id.ShouldNotBe(Guid.Empty);
        refundRequest.UserId.ShouldBe(userId);
        refundRequest.TicketId.ShouldBe(validTicket.Id);
        refundRequest.Ticket.ShouldBeSameAs(validTicket); // Check instance equality
        refundRequest.Reason.ShouldBe(reason);
        refundRequest.CreatedAt.ShouldBe(expectedCreationTime.DateTime);
        refundRequest.DomainEvents.ShouldContain(e => e is RefundRequestCreated);
        ((RefundRequestCreated)refundRequest.DomainEvents.First()).OperatorId.ShouldBe(validTicket.OperatorId);
    }

    [Fact]
    public void Create_Should_CreateRefundRequest_For_ValidDataWithoutUserId()
    {
        // Arrange
        Guid? userId = null;
        var validTicket = CreateValidTicket(); // Create a real ticket
        var reason = "Flight got cancelled";
        var expectedCreationTime = _fakeTimeProvider.GetUtcNow();

        // Act
        var refundRequest = RefundRequest.Create(userId, validTicket, reason, _fakeTimeProvider);

        // Assert
        refundRequest.ShouldNotBeNull();
        refundRequest.Id.ShouldNotBe(Guid.Empty);
        refundRequest.UserId.ShouldBeNull();
        refundRequest.TicketId.ShouldBe(validTicket.Id);
        refundRequest.Ticket.ShouldBeSameAs(validTicket);
        refundRequest.Reason.ShouldBe(reason);
        refundRequest.CreatedAt.ShouldBe(expectedCreationTime.DateTime);
        refundRequest.DomainEvents.ShouldContain(e => e is RefundRequestCreated);
    }

    [Theory]
    [InlineData("")] // Empty string is invalid due to MinimumLength(3)
    [InlineData("AB")] // Too short (MinLength is 3)
    public void Create_Should_ThrowValidationException_For_InvalidReason(string invalidReason)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var validTicket = CreateValidTicket(); // Create a real ticket
        _fakeTimeProvider.SetUtcNow(DateTimeOffset.UtcNow);

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            RefundRequest.Create(userId, validTicket, invalidReason, _fakeTimeProvider));
    }

    [Fact]
    public void Create_Should_ThrowValidationException_For_TooLongReason()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var validTicket = CreateValidTicket(); // Create a real ticket
        var tooLongReason = new string('X', 256); // Max length is 255

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            RefundRequest.Create(userId, validTicket, tooLongReason, _fakeTimeProvider));
    }

    [Fact]
    public void Accept_Should_UpdateTicketStatusAndTimestamp()
    {
        // Arrange
        var validTicket = CreateValidTicket(); // Create a real ticket
        var refundRequest = RefundRequest.Create(Guid.NewGuid(), validTicket, "Valid Reason", _fakeTimeProvider);
        validTicket.ClearDomainEvents(); // Clear ticket creation events if any

        var expectedAcceptTime = _fakeTimeProvider.GetUtcNow();

        // Act
        refundRequest.Accept(_fakeTimeProvider);

        // Assert
        // Verify the state of the real Ticket object
        validTicket.TicketStatus.ShouldBe(TicketStatus.REFUNDED);
        validTicket.RefundedAt.ShouldBe(expectedAcceptTime.DateTime);
        validTicket.DomainEvents.Count.ShouldBe(1); // Should contain TicketRefundedEvent
        validTicket.DomainEvents.First().ShouldBeOfType<TicketRefundedEvent>();
        ((TicketRefundedEvent)validTicket.DomainEvents.First()).TicketId.ShouldBe(validTicket.Id);
    }

    [Fact]
    public void Reject_Should_AddRefundRejectedEvent()
    {
        // Arrange
        var validTicket = CreateValidTicket(); // Create a real ticket
        var refundRequest = RefundRequest.Create(Guid.NewGuid(), validTicket, "Valid Reason", _fakeTimeProvider);
        refundRequest.ClearDomainEvents(); // Clear initial RefundRequestCreated event

        // Act
        refundRequest.Reject();

        // Assert
        refundRequest.DomainEvents.Count.ShouldBe(1);
        var domainEvent = refundRequest.DomainEvents.First();
        domainEvent.ShouldBeOfType<RefundRejectedEvent>();
        var rejectedEvent = (RefundRejectedEvent)domainEvent;
        rejectedEvent.Id.ShouldBe(refundRequest.Id); // Corrected property name
        rejectedEvent.TicketId.ShouldBe(validTicket.Id);
    }
}