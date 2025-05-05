using FluentValidation;
using Microsoft.Extensions.Time.Testing;
using Shouldly;
using TrackEasy.Domain.Shared;
using TrackEasy.Domain.Tickets;
// Added for FakeTimeProvider

// Added Xunit for Fact/Theory

namespace TrackEasy.UnitTests.Tickets;

public class TicketTests
{
    private readonly FakeTimeProvider _fakeTimeProvider; // Changed type to FakeTimeProvider

    public TicketTests()
    {
        // Initialize FakeTimeProvider with a starting time
        _fakeTimeProvider = new FakeTimeProvider(new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero));
    }

    private static List<TicketConnectionStation> CreateValidStations() =>
    [
        new("Station A", null, new TimeOnly(10, 0), 1),
        new("Station B", new TimeOnly(11, 0), null, 2)
    ];

    private static List<Person> CreateValidPassengers(int count = 1) =>
        Enumerable.Range(1, count)
            .Select(i => new Person($"First{i}", $"Last{i}", new DateOnly(1990 + i, 1, 1), null))
            .ToList();

    // Assuming Currency.USD exists or is defined elsewhere
    private static Money CreateValidPrice() => new(100, Currency.USD);

    // Modified to accept and use the class-level FakeTimeProvider
    private Ticket CreateValidTicket(Guid? passengerId = null, List<int>? seatNumbers = null) =>
        Ticket.Create(
            Guid.NewGuid(),
            "Express 123",
            CreateValidStations(),
            CreateValidPassengers(seatNumbers?.Count ?? 1), // Ensure passenger count matches seat count if provided
            seatNumbers ?? [1], // Default to 1 seat if null
            CreateValidPrice(),
            Guid.NewGuid(),
            "OP-CODE",
            "Operator Name",
            passengerId,
            _fakeTimeProvider // Pass the class-level provider
        );

    [Fact]
    public void Create_Should_CreateTicket_For_ValidData()
    {
        // Arrange
        var connectionId = Guid.NewGuid();
        var connectionName = "InterCity 456";
        var stations = CreateValidStations();
        var passengers = CreateValidPassengers(2);
        var seatNumbers = new List<int> { 10, 11 };
        var price = CreateValidPrice();
        var operatorId = Guid.NewGuid();
        var operatorCode = "OP-XYZ";
        var operatorName = "Test Operator";
        var passengerId = Guid.NewGuid();
        var expectedCreationTime = _fakeTimeProvider.GetUtcNow();

        // Act
        var ticket = Ticket.Create(connectionId, connectionName, stations, passengers, seatNumbers, price, operatorId, operatorCode, operatorName, passengerId, _fakeTimeProvider);

        // Assert
        ticket.ShouldNotBeNull();
        ticket.Id.ShouldNotBe(Guid.Empty);
        ticket.ConnectionId.ShouldBe(connectionId);
        ticket.ConnectionName.ShouldBe(connectionName);
        ticket.Stations.ShouldBe(stations);
        ticket.Passengers.ShouldBe(passengers);
        ticket.SeatNumbers.ShouldBe(seatNumbers);
        ticket.Price.ShouldBe(price);
        ticket.OperatorId.ShouldBe(operatorId);
        ticket.OperatorCode.ShouldBe(operatorCode);
        ticket.OperatorName.ShouldBe(operatorName);
        ticket.PassengerId.ShouldBe(passengerId);
        ticket.TicketStatus.ShouldBe(TicketStatus.PENDING);
        ticket.CreatedAt.ShouldBe(expectedCreationTime.DateTime);
        ticket.PaidAt.ShouldBeNull();
        ticket.CanceledAt.ShouldBeNull();
        ticket.RefundedAt.ShouldBeNull();
    }

    [Theory]
    [InlineData("", "Valid Name", "Valid Code")] // Invalid Connection Name
    [InlineData("Valid Connection", "", "Valid Code")] // Invalid Operator Name
    [InlineData("Valid Connection", "Valid Name", "")] // Invalid Operator Code
    public void Create_Should_ThrowValidationException_For_InvalidStringProperties(string connectionName, string operatorName, string operatorCode)
    {
        // Arrange
        var stations = CreateValidStations();
        var passengers = CreateValidPassengers();
        var price = CreateValidPrice();

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Ticket.Create(Guid.NewGuid(), connectionName, stations, passengers, [1], price, Guid.NewGuid(), operatorCode, operatorName, null, _fakeTimeProvider));
    }

    [Fact]
    public void Create_Should_ThrowValidationException_For_EmptyStations()
    {
        // Arrange
        var passengers = CreateValidPassengers();
        var price = CreateValidPrice();

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Ticket.Create(Guid.NewGuid(), "Valid Connection", [], passengers, [1], price, Guid.NewGuid(), "CODE", "Name", null, _fakeTimeProvider))
            .Message.ShouldContain("'Stations' must not be empty");
    }

     [Fact]
    public void Create_Should_ThrowValidationException_For_EmptyPassengers()
    {
        // Arrange
        var stations = CreateValidStations();
        var price = CreateValidPrice();

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Ticket.Create(Guid.NewGuid(), "Valid Connection", stations, [], [1], price, Guid.NewGuid(), "CODE", "Name", null, _fakeTimeProvider))
            .Message.ShouldContain("'Passengers' must not be empty");
    }

    [Fact]
    public void Create_Should_ThrowValidationException_When_SeatCount_DoesNotMatch_PassengerCount()
    {
        // Arrange
        var stations = CreateValidStations();
        var passengers = CreateValidPassengers(2); // 2 passengers
        var seatNumbers = new List<int> { 1 }; // 1 seat
        var price = CreateValidPrice();

        // Act & Assert
        Should.Throw<ValidationException>(() =>
            Ticket.Create(Guid.NewGuid(), "Valid Connection", stations, passengers, seatNumbers, price, Guid.NewGuid(), "CODE", "Name", null, _fakeTimeProvider));
    }


    [Fact]
    public void Pay_Should_UpdateStatusAndTimestamp_And_AddEvent()
    {
        // Arrange
        var ticket = CreateValidTicket(); // Uses _fakeTimeProvider for creation time
        var transactionId = Guid.NewGuid();
        var expectedPayTime = new DateTimeOffset(2024, 1, 1, 13, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(expectedPayTime); // Set time for Pay operation
        ticket.ClearDomainEvents();

        // Act
        ticket.Pay(_fakeTimeProvider, transactionId);

        // Assert
        ticket.TicketStatus.ShouldBe(TicketStatus.PAID);
        ticket.PaidAt.ShouldBe(expectedPayTime.DateTime);
        ticket.TransactionId.ShouldBe(transactionId);
        ticket.DomainEvents.Count.ShouldBe(1);
        ticket.DomainEvents.First().ShouldBeOfType<TicketPayedEvent>();
        ((TicketPayedEvent)ticket.DomainEvents.First()).TicketId.ShouldBe(ticket.Id);
    }

    [Fact]
    public void Cancel_Should_UpdateStatusAndTimestamp_And_AddEvent()
    {
        // Arrange
        var ticket = CreateValidTicket();
        var expectedCancelTime = new DateTimeOffset(2024, 1, 2, 9, 0, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(expectedCancelTime); // Set time for Cancel operation
        ticket.ClearDomainEvents();

        // Act
        ticket.Cancel(_fakeTimeProvider);

        // Assert
        ticket.TicketStatus.ShouldBe(TicketStatus.CANCELED);
        ticket.CanceledAt.ShouldBe(expectedCancelTime.DateTime);
        ticket.PaidAt.ShouldBeNull();
        ticket.RefundedAt.ShouldBeNull();
        ticket.DomainEvents.Count.ShouldBe(1);
        ticket.DomainEvents.First().ShouldBeOfType<TicketCanceledEvent>();
        ((TicketCanceledEvent)ticket.DomainEvents.First()).TicketId.ShouldBe(ticket.Id);
    }

    [Fact]
    public void Refund_Should_UpdateStatusAndTimestamp_And_AddEvent()
    {
        // Arrange
        var ticket = CreateValidTicket();
        var expectedRefundTime = new DateTimeOffset(2024, 1, 3, 15, 30, 0, TimeSpan.Zero);
        _fakeTimeProvider.SetUtcNow(expectedRefundTime); // Set time for Refund operation
        ticket.ClearDomainEvents();

        // Act
        ticket.Refund(_fakeTimeProvider); // Note: Refund is internal

        // Assert
        ticket.TicketStatus.ShouldBe(TicketStatus.REFUNDED);
        ticket.RefundedAt.ShouldBe(expectedRefundTime.DateTime);
        ticket.PaidAt.ShouldBeNull();
        ticket.CanceledAt.ShouldBeNull();
        ticket.DomainEvents.Count.ShouldBe(1);
        ticket.DomainEvents.First().ShouldBeOfType<TicketRefundedEvent>();
        ((TicketRefundedEvent)ticket.DomainEvents.First()).TicketId.ShouldBe(ticket.Id);
    }
}