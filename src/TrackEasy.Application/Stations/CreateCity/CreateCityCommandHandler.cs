using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Stations.CreateCity;

public sealed class CreateCityCommandHandler(ICityRepository cityRepository)
    : ICommandHandler<CreateCityCommand>
{
    public async Task Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var exists = await cityRepository.ExistsAsync(request.Name, cancellationToken);

        if (exists)
        {
            throw new TrackEasyException(Codes.CityAlreadyExists, $"City '{request.Name}' already exists.");
        }

        var city = City.Create(request.Name, request.Country);
        cityRepository.Add(city);
        await cityRepository.SaveChangesAsync(cancellationToken);
    }
}
