using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Stations;

public interface IStationRepository : IBaseRepository
{
    Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);
    Task<Station?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(Station station);
    void Delete(Station station);
}