using TrackEasy.Domain.Cities;
using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.CreateStation;

internal sealed class CreateStationCommandHandler(IStationRepository stationRepository, ICityRepository cityRepository) 
    : ICommandHandler<CreateStationCommand, Guid>
{
    public Task<Guid> Handle(CreateStationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}