using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Discounts;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class DiscountRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IDiscountRepository
{
    private readonly TrackEasyDbContext _dbContext = dbContext;
    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
        => _dbContext.Discounts.AnyAsync(x => x.Name == name, cancellationToken);

    public async Task<Discount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _dbContext.Discounts.FindAsync([id], cancellationToken);
    
    public void Add(Discount discount)
    {
        _dbContext.Discounts.Add(discount);
    }
    public void Delete(Discount discount)
    {
        _dbContext.Discounts.Remove(discount);
    }
}