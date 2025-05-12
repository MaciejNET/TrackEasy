using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Stations.DeleteStation;

internal sealed class DeleteStationCommandHandler(IStationRepository stationRepository)
    : ICommandHandler<DeleteStationCommand>
{
    public async Task Handle(DeleteStationCommand request, CancellationToken cancellationToken)
    {
        var station = await stationRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (station is null)
            throw new TrackEasyException(
                SharedCodes.EntityNotFound,
                $"Station with id '{request.Id}' not found.");
        
        stationRepository.Delete(station);
        await stationRepository.SaveChangesAsync(cancellationToken);
    }
}