using MediatR;
using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Stations.Cities;
using TrackEasy.Infrastructure.Persistence;

namespace TrackEasy.Infrastructure.Stations.Cities.Queries;

public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, IEnumerable<CityDto>>
{
    private readonly TrackEasyDbContext _dbContext;

    public GetCitiesQueryHandler(TrackEasyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Cities
            .AsNoTracking()
            .Select(c => new CityDto
            {
                Id = c.Id,
                Name = c.Name,
                CountryId = c.CountryId
            })
            .ToListAsync(cancellationToken);
    }
}
