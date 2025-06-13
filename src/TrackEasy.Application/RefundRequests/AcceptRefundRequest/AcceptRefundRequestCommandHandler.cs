using TrackEasy.Domain.RefundRequests;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.RefundRequests.AcceptRefundRequest;

internal sealed class AcceptRefundRequestCommandHandler(
    IRefundRequestRepository refundRequestRepository,
    TimeProvider timeProvider)
    : ICommandHandler<AcceptRefundRequestCommand>
{
    public async Task Handle(AcceptRefundRequestCommand request, CancellationToken cancellationToken)
    {
        var refundRequest = await refundRequestRepository.FindByIdAsync(request.Id, cancellationToken);
        
        if (refundRequest is null)
        {
            throw new TrackEasyException(
                SharedCodes.EntityNotFound,
                $"Refund request with id '{request.Id}' was not found.");
        }
        
        refundRequest.Accept(timeProvider);
        refundRequestRepository.Delete(refundRequest);
        await refundRequestRepository.SaveChangesAsync(cancellationToken);
    }
}
