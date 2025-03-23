using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.DiscountCodes;

public interface IDiscountCodeRepository : IBaseRepository
{
    Task<bool> ExistsAsync(string code, CancellationToken cancellationToken);
    Task<DiscountCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(DiscountCode discountCode);
    void Delete(DiscountCode discountCode);
}