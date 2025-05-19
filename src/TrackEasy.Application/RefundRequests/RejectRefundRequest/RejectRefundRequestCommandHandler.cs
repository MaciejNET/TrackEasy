using TrackEasy.Domain.RefundRequests;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.RefundRequests.RejectRefundRequest;

internal sealed class RejectRefundRequestCommandHandler(IRefundRequestRepository refundRequestRepository, TimeProvider timeProvider)
    : ICommandHandler<RejectRefundRequestCommand>
{
    public Task Handle(RejectRefundRequestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}