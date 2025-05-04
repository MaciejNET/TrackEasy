using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.DiscountCodes;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class DiscountCodeRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IDiscountCodeRepository
{
    private readonly TrackEasyDbContext _dbContext = dbContext;
    public Task<bool> ExistsAsync(string code, CancellationToken cancellationToken)
        => _dbContext.DiscountCodes.AnyAsync(x => x.Code == code, cancellationToken);

    public async Task<DiscountCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _dbContext.DiscountCodes.FindAsync([id], cancellationToken);

    public void Add(DiscountCode discountCode)
    {
        _dbContext.DiscountCodes.Add(discountCode);
    }
    
    public void Delete(DiscountCode discountCode)
    {
        _dbContext.DiscountCodes.Remove(discountCode);
    }
}