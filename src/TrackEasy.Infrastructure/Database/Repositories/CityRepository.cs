using TrackEasy.Domain.Entities;
using TrackEasy.Domain.Stations;
using Microsoft.EntityFrameworkCore;
using TrackEasy.Infrastructure.Persistence;

namespace TrackEasy.Infrastructure.Repositories;


public class CityRepository : ICityRepository
{
    private readonly ApplicationDbContext _context;

    public CityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<City?> GetByIdAsync(Guid id)
    {
        return await _context.Cities.FindAsync(id);
    }

    public async Task<IEnumerable<City>> GetAllAsync()
    {
        return await _context.Cities.ToListAsync();
    }

    public async Task AddAsync(City city)
    {
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(City city)
    {
        _context.Cities.Update(city);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city != null)
        {
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
        }
    }
}
