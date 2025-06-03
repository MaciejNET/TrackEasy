using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Stations;

public interface IStationRepository : IBaseRepository
{
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, string name, CancellationToken cancellationToken);
    Task<Station?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Station>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
    void Add(Station station);
    void Delete(Station station);
}