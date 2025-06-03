using TrackEasy.Domain.Tickets;
using TrackEasy.Mails.Abstractions;
using TrackEasy.Mails.Abstractions.Models;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.TicketCanceled;

internal sealed class TicketCanceledEventHandler(ITicketRepository ticketRepository, IEmailSender emailSender) : IDomainEventHandler<TicketCanceledEvent>
{
    public async Task Handle(TicketCanceledEvent notification, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(notification.TicketId, cancellationToken);
        if (ticket is null)
        {
            return;
        }

        await emailSender.SendTicketCancelledEmailAsync(ticket.EmailAddress, new TicketCancelledModel(ticket.TicketNumber));
    }
}