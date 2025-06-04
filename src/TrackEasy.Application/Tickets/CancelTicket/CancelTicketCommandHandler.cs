using TrackEasy.Domain.Tickets;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Tickets.CancelTicket;

internal sealed class CancelTicketCommandHandler(ITicketRepository ticketRepository, TimeProvider timeProvider) : ICommandHandler<CancelTicketCommand>
{
    public async Task Handle(CancelTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(request.TicketId, cancellationToken);
        
        if (ticket is null)
        {
            throw new TrackEasyException(
                SharedCodes.EntityNotFound,
                $"Ticket with id '{request.TicketId}' was not found.");
        }
        
        ticket.Cancel(timeProvider);
        
        await ticketRepository.SaveChangesAsync(cancellationToken);
    }
}