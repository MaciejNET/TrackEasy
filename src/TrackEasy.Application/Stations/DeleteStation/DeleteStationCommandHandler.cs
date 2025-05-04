using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.DeleteStation;

internal sealed class DeleteStationCommandHandler(IStationRepository stationRepository) : ICommandHandler<DeleteStationCommand>
{
    public Task Handle(DeleteStationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}