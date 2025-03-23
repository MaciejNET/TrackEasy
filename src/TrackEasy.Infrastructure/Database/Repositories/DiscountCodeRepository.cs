using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.DiscountCodes;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class DiscountCodeRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IDiscountCodeRepository
{
    public Task<bool> ExistsAsync(string code, CancellationToken cancellationToken)
        => dbContext.DiscountCodes.AnyAsync(x => x.Code == code, cancellationToken);

    public async Task<DiscountCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await dbContext.DiscountCodes.FindAsync([id], cancellationToken);

    public void Add(DiscountCode discountCode)
    {
        dbContext.DiscountCodes.Add(discountCode);
    }

    public void Delete(DiscountCode discountCode)
    {
        dbContext.DiscountCodes.Remove(discountCode);
    }
}