using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.RefundRequests;

public interface IRefundRequestRepository : IBaseRepository
{
    Task<RefundRequest?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(RefundRequest refundRequest);
    void Delete(RefundRequest refundRequest);
}