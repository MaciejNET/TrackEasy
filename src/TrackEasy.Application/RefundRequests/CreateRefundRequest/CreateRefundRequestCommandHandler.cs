using TrackEasy.Domain.RefundRequests;
using TrackEasy.Domain.Tickets;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.RefundRequests.CreateRefundRequest;

internal sealed class CreateRefundRequestCommandHandler(
    IRefundRequestRepository refundRequestRepository,
    ITicketRepository ticketRepository,
    TimeProvider timeProvider) 
    : ICommandHandler<CreateRefundRequestCommand, Guid>
{
    public Task<Guid> Handle(CreateRefundRequestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}