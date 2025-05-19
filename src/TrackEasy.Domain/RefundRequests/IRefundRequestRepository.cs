namespace TrackEasy.Domain.RefundRequests;

public interface IRefundRequestRepository
{
    Task<RefundRequest?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(RefundRequest refundRequest);
}