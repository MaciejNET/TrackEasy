using TrackEasy.Domain.Tickets;
using TrackEasy.Mails.Abstractions;
using TrackEasy.Mails.Abstractions.Models;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.TicketCreatedForExternalUser;

internal sealed class TicketCreatedForExternalUserEventHandler(ITicketRepository ticketRepository, IEmailSender emailSender) : IDomainEventHandler<TicketCreatedForExternalUserEvent>
{
    public async Task Handle(TicketCreatedForExternalUserEvent notification, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(notification.TicketId, cancellationToken);
        if (ticket is null)
        {
            return;
        }

        var model = new TicketNumberModel(ticket.TicketNumber);
        await emailSender.SendTicketNumberEmailAsync(ticket.EmailAddress, model);
    }
}