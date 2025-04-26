using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Cities;

public interface ICityRepository : IBaseRepository
{
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);
    Task<City?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(City city);
    void Delete(City city);
}
