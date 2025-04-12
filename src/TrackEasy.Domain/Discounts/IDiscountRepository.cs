using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Discounts;

public interface IDiscountRepository : IBaseRepository
{
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);
    Task<Discount?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(Discount discount);
    void Delete(Discount discount);
}
