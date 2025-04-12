using MediatR;
using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Stations.Cities;
using TrackEasy.Infrastructure.Persistence;

namespace TrackEasy.Infrastructure.Stations.Cities.Queries;

public class FindCityQueryHandler : IRequestHandler<FindCityQuery, CityDto?>
{
    private readonly TrackEasyDbContext _dbContext;

    public FindCityQueryHandler(TrackEasyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CityDto?> Handle(FindCityQuery request, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (city == null) return null;

        return new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            CountryId = city.CountryId
        };
    }
}
