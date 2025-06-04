using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.RefundRequests;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class RefundRequestRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IRefundRequestRepository
{
    private readonly TrackEasyDbContext _dbContext = dbContext;
    
    public Task<RefundRequest?> FindByIdAsync(Guid id, CancellationToken cancellationToken) 
        => _dbContext.RefundRequests
            .Include(x => x.Ticket)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Add(RefundRequest refundRequest)
    {
        _dbContext.RefundRequests.Add(refundRequest);
    }
}