using TrackEasy.Domain.RefundRequests;
using TrackEasy.Domain.Tickets;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.RefundRequests.CreateRefundRequest;

internal sealed class CreateRefundRequestCommandHandler(
    IRefundRequestRepository refundRequestRepository,
    ITicketRepository ticketRepository,
    TimeProvider timeProvider)
    : ICommandHandler<CreateRefundRequestCommand, Guid>
{
    public async Task<Guid> Handle(CreateRefundRequestCommand request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(request.TicketId, cancellationToken);
        
        if (ticket is null)
        {
            throw new TrackEasyException(
                SharedCodes.EntityNotFound,
                $"Ticket with id '{request.TicketId}' was not found.");
        }
        
        var refundRequest = RefundRequest.Create(
            request.UserId,
            ticket,
            request.Reason,
            timeProvider);
        
        refundRequestRepository.Add(refundRequest);
        await refundRequestRepository.SaveChangesAsync(cancellationToken);

        return refundRequest.Id;
    }
}