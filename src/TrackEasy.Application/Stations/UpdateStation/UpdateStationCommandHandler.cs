using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Stations.UpdateStation;

internal sealed class UpdateStationCommandHandler(IStationRepository stationRepository)
    : ICommandHandler<UpdateStationCommand>
{
    public async Task Handle(UpdateStationCommand request, CancellationToken cancellationToken)
    {
        var station = await stationRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (station is null)
            throw new TrackEasyException(
                SharedCodes.EntityNotFound,
                $"Station with id '{request.Id}' not found.");
        
        var nameTaken = await stationRepository.ExistsAsync(request.Id, request.Name, cancellationToken);
        
        if (nameTaken)
            throw new TrackEasyException(
                Codes.StationAlreadyExists,
                $"Station with name '{request.Name}' already exists.");
        
        var dto  = request.GeographicalCoordinates;
        var coords = new GeographicalCoordinates(dto.Latitude, dto.Longitude);
        
        station.Update(request.Name, coords);
        
        await stationRepository.SaveChangesAsync(cancellationToken);
    }
}