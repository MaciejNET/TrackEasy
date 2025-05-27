using TrackEasy.Domain.RefundRequests;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.RefundRequests.RejectRefundRequest;

internal sealed class RejectRefundRequestCommandHandler(
    IRefundRequestRepository refundRequestRepository,
    TimeProvider timeProvider)
    : ICommandHandler<RejectRefundRequestCommand>
{
    public async Task Handle(RejectRefundRequestCommand request, CancellationToken cancellationToken)
    {
        var refundRequest = await refundRequestRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (refundRequest is null)
        {
            throw new TrackEasyException(
                SharedCodes.EntityNotFound,
                $"Refund request with id '{request.Id}' was not found.");
        }
        
        refundRequest.Reject();
        
        await refundRequestRepository.SaveChangesAsync(cancellationToken);
    }
}