using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Cities.CreateCity;

public sealed class CreateCityCommandHandler(ICityRepository cityRepository)
    : ICommandHandler<CreateCityCommand, Guid>
{
    public async Task<Guid> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var exists = await cityRepository.ExistsAsync(request.Name, cancellationToken);

        if (exists)
        {
            throw new TrackEasyException(Codes.CityAlreadyExists, $"City '{request.Name}' already exists.");
        }

        var city = City.Create(request.Name, request.Country, request.FunFacts);
        cityRepository.Add(city);
        await cityRepository.SaveChangesAsync(cancellationToken);
        
        return city.Id;
    }
}
