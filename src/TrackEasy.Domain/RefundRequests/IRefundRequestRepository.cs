namespace TrackEasy.Domain.RefundRequests;

using TrackEasy.Shared.Domain.Abstractions;

public interface IRefundRequestRepository : IBaseRepository
{
    Task<RefundRequest?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(RefundRequest refundRequest);
}