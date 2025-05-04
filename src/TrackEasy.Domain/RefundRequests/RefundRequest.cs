using FluentValidation;
using TrackEasy.Domain.Tickets;
using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.RefundRequests;

public sealed class RefundRequest : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid? UserId { get; private set; }
    public Guid TicketId { get; private set; }
    public Ticket Ticket { get; private set; }
    public string Reason { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static RefundRequest Create(Guid? userId, Ticket ticket, string reason, TimeProvider timeProvider)
    {
        var refundRequest = new RefundRequest
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TicketId = ticket.Id,
            Ticket = ticket,
            Reason = reason,
            CreatedAt = timeProvider.GetUtcNow().DateTime
        };
        
        new RefundRequestValidator().ValidateAndThrow(refundRequest);
        refundRequest.AddDomainEvent(new RefundRequestCreated(ticket.OperatorId));
        return refundRequest;
    }
    
    public void Accept(TimeProvider timeProvider)
    {
        Ticket.Refund(timeProvider);
    }
    
    public void Reject()
    {
        AddDomainEvent(new RefundRejectedEvent(Id, TicketId));
    }
    
#pragma warning disable CS8618
    private RefundRequest() { }
#pragma warning restore CS8618
}