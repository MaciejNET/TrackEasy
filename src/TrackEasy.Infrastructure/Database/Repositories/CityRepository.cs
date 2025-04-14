using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Stations;
using TrackEasy.Infrastructure.Database;

namespace TrackEasy.Infrastructure.Repositories;

internal sealed class CityRepository(TrackEasyDbContext dbContext) : ICityRepository
{
    public void Add(City city) => dbContext.Cities.Add(city);

    public void Delete(City city) => dbContext.Cities.Remove(city);

    public async Task<IReadOnlyList<City>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Cities.ToListAsync(cancellationToken);
    }

    public async Task<City?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Cities.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
    {
        return await dbContext.Cities.AnyAsync(x => x.Name == name, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
