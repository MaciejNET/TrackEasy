using TrackEasy.Domain.Tickets;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Tickets.PayTicketByCash;

internal sealed class PayTicketByCashCommandHandler(ITicketRepository ticketRepository, TimeProvider timeProvider) : ICommandHandler<PayTicketByCashCommand>
{
    public async Task Handle(PayTicketByCashCommand request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByNumberAsync(request.TicketNumber, cancellationToken);

        if (ticket is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Ticket with number: {request.TicketNumber} was not found.");
        }

        ticket.Pay(timeProvider);

        await ticketRepository.SaveChangesAsync(cancellationToken);
    }
}