using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class StationRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IStationRepository
{
    private readonly TrackEasyDbContext _dbContext = dbContext;
    
    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
        => _dbContext.Stations.AnyAsync(x => x.Name == name, cancellationToken);

    public async Task<Station?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _dbContext.Stations.FindAsync([id], cancellationToken);

    public void Add(Station station)
    {
        _dbContext.Stations.Add(station);
    }

    public void Delete(Station station)
    {
        _dbContext.Stations.Remove(station);
    }
}