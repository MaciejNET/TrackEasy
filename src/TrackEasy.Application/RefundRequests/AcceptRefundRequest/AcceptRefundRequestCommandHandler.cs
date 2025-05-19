using TrackEasy.Domain.RefundRequests;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.RefundRequests.AcceptRefundRequest;

internal sealed class AcceptRefundRequestCommandHandler(IRefundRequestRepository refundRequestRepository, TimeProvider timeProvider)
    : ICommandHandler<AcceptRefundRequestCommand>
{
    public Task Handle(AcceptRefundRequestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}