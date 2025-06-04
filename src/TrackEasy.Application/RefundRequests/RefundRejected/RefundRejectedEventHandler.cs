using TrackEasy.Domain.RefundRequests;
using TrackEasy.Domain.Tickets;
using TrackEasy.Mails.Abstractions;
using TrackEasy.Mails.Abstractions.Models;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.RefundRequests.RefundRejected;

internal sealed class RefundRejectedEventHandler(ITicketRepository ticketRepository, IEmailSender emailSender) : IDomainEventHandler<RefundRejectedEvent>
{
    public async Task Handle(RefundRejectedEvent notification, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(notification.TicketId, cancellationToken);
        if (ticket is null)
        {
            return;
        }

        await emailSender.SendRefundRejectedEmailAsync(ticket.EmailAddress, new RefundRejectedModel(ticket.TicketNumber));
    }
}