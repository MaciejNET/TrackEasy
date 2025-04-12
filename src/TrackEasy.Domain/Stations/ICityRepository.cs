using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Domain.Stations;

public interface ICityRepository
{
    Task<City> GetByIdAsync(Guid id);
    Task<IEnumerable<City>> GetAllAsync();
    Task AddAsync(City city);
    Task UpdateAsync(City city);
    Task DeleteAsync(Guid id);
}
