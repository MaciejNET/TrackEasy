using TrackEasy.Domain.Tickets;
using TrackEasy.Mails.Abstractions;
using TrackEasy.Mails.Abstractions.Models;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.TicketCreatedForExternalUser;

internal sealed class TicketCreatedForExternalUserEventHandler(IEmailSender emailSender) : IDomainEventHandler<TicketCreatedForExternalUserEvent>
{
    public async Task Handle(TicketCreatedForExternalUserEvent notification, CancellationToken cancellationToken)
    {
        var model = new TicketNumberModel(notification.Ticket.TicketNumber);
        await emailSender.SendTicketNumberEmailAsync(notification.Ticket.EmailAddress, model);
    }
}