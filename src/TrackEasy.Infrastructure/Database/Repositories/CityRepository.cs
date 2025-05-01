using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Cities;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class CityRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), ICityRepository
{
    public async Task<City?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Cities.FindAsync([id], cancellationToken);
    }

    public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
    {
        return dbContext.Cities.AnyAsync(x => x.Name == name, cancellationToken);
    }

    public void Add(City city) => dbContext.Cities.Add(city);

    public void Delete(City city) => dbContext.Cities.Remove(city);
}
