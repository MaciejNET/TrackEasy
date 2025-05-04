using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Cities;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class CityRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), ICityRepository
{
    private readonly TrackEasyDbContext _dbContext = dbContext;
    public async Task<City?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Cities.FindAsync([id], cancellationToken);
    }

    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
    {
        return _dbContext.Cities.AnyAsync(x => x.Name == name, cancellationToken);
    }

    public void Add(City city) => _dbContext.Cities.Add(city);

    public void Delete(City city) => _dbContext.Cities.Remove(city);
}
