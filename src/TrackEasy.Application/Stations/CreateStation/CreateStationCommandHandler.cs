using TrackEasy.Domain.Cities;
using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Stations.CreateStation;

internal sealed class CreateStationCommandHandler(IStationRepository stationRepository, ICityRepository    cityRepository)
    : ICommandHandler<CreateStationCommand, Guid>
{
    public async Task<Guid> Handle(CreateStationCommand request, CancellationToken cancellationToken)
    {
        if (await stationRepository.ExistsAsync(request.Name, cancellationToken))
            throw new TrackEasyException(
                Codes.StationAlreadyExists,
                $"Station with name '{request.Name}' already exists.");
        
        var city = await cityRepository.GetByIdAsync(request.CityId, cancellationToken);
        
        if (city is null)
            throw new TrackEasyException(
                SharedCodes.EntityNotFound,
                $"City with id '{request.CityId}' not found.");
        
        var geoDto = request.GeographicalCoordinates;
        var geo    = new GeographicalCoordinates(geoDto.Latitude, geoDto.Longitude);
        
        var station = Station.Create(request.Name, city, geo);
        
        stationRepository.Add(station);
        await stationRepository.SaveChangesAsync(cancellationToken);

        return station.Id;
    }
}
