using FluentValidation;
using TrackEasy.Domain.Shared;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Ticket;

public sealed class Ticket : AggregateRoot
{
    public Guid Id { get; private set; }
    public int TicketNumber { get; private set; }
    public Guid? PassengerId { get; private set; }
    public Guid? TransactionId { get; private set; }
    public Money Price { get; private set; }
    public TicketStatus TicketStatus { get; private set; }
    public Guid ConnectionId { get; private set; }
    public string ConnectionName { get; private set; }
    public IReadOnlyList<int>? SeatNumbers { get; private set; }
    public Guid OperatorId { get; private set; }
    public string OperatorCode { get; private set; }
    public string OperatorName { get; private set; }
    public IReadOnlyList<TicketConnectionStation> Stations { get; private set; }
    public IReadOnlyList<Person> Passengers { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? CanceledAt { get; private set; }
    public DateTime? RefundedAt { get; private set; }
    
    public static Ticket Create(Guid connectionId, string connectionName, IEnumerable<TicketConnectionStation> stations,
        IEnumerable<Person> passengers, IEnumerable<int>? seatNumbers, Money price, Guid operatorId,
        string operatorCode, string operatorName, Guid? passengerId, TimeProvider timeProvider)
    {
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            PassengerId = passengerId,
            ConnectionId = connectionId,
            ConnectionName = connectionName,
            Stations = [..stations],
            Passengers = [..passengers],
            SeatNumbers = seatNumbers?.ToList(),
            Price = price,
            TicketStatus = TicketStatus.PENDING,
            OperatorId = operatorId,
            OperatorCode = operatorCode,
            OperatorName = operatorName,
            CreatedAt = timeProvider.GetUtcNow().DateTime
        };
        
        new TicketValidator().ValidateAndThrow(ticket);
        return ticket;
    }
    
    public void Pay(TimeProvider timeProvider, Guid? transactionId = null)
    {
        TicketStatus = TicketStatus.PAID;
        PaidAt = timeProvider.GetUtcNow().DateTime;
        TransactionId = transactionId;
        new TicketValidator().ValidateAndThrow(this);
        AddDomainEvent(new TicketPayedEvent(Id));
    }
    
    public void Cancel(TimeProvider timeProvider)
    {
        TicketStatus = TicketStatus.CANCELED;
        CanceledAt = timeProvider.GetUtcNow().DateTime;
        new TicketValidator().ValidateAndThrow(this);
        AddDomainEvent(new TicketCanceledEvent(Id));
    }
    
    public void Refund(TimeProvider timeProvider)
    {
        TicketStatus = TicketStatus.REFUNDED;
        RefundedAt = timeProvider.GetUtcNow().DateTime;
        new TicketValidator().ValidateAndThrow(this);
        AddDomainEvent(new TicketRefundedEvent(Id));
    }
    
#pragma warning disable CS8618
    private Ticket() {}
#pragma warning restore CS8618
}