using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Discounts;
using TrackEasy.Infrastructure.Database.Repositories;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class DiscountRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IDiscountRepository
{
    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
        => dbContext.Discounts.AnyAsync(x => x.Name == name, cancellationToken);

    public async Task<Discount?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await dbContext.Discounts.FindAsync([id], cancellationToken);
    
    public void Add(Discount discount)
    {
        dbContext.Discounts.Add(discount);
    }
    public void Delete(Discount discount)
    {
        dbContext.Discounts.Remove(discount);
    }
}