using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Stations.DeleteCity;

internal sealed class DeleteCityCommandHandler(ICityRepository cityRepository)
    : ICommandHandler<DeleteCityCommand>
{
    public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var city = await cityRepository.GetByIdAsync(request.Id, cancellationToken);

        if (city is null)
        {
            throw new TrackEasyException("CityNotFound", $"City with id: {request.Id} does not exist.");
        }

        cityRepository.Delete(city);
        await cityRepository.SaveChangesAsync(cancellationToken);
    }
}
