using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.UpdateStation;

internal sealed class UpdateStationCommandHandler(IStationRepository stationRepository) : ICommandHandler<UpdateStationCommand>
{
    public Task Handle(UpdateStationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}