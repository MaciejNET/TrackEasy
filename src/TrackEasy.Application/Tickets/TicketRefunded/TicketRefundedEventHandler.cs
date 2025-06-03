using TrackEasy.Domain.Tickets;
using TrackEasy.Mails.Abstractions;
using TrackEasy.Mails.Abstractions.Models;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.TicketRefunded;

internal sealed class TicketRefundedEventHandler(ITicketRepository ticketRepository, IEmailSender emailSender) : IDomainEventHandler<TicketRefundedEvent>
{
    public async Task Handle(TicketRefundedEvent notification, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(notification.TicketId, cancellationToken);
        if (ticket is null)
        {
            return;
        }
        
        await emailSender.SendTicketRefundedEmailAsync(ticket.EmailAddress, new TicketRefundedModel(ticket.TicketNumber));
    }
}