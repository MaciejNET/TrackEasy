using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Cities.UpdateCity;

internal sealed class UpdateCityCommandHandler(ICityRepository cityRepository)
    : ICommandHandler<UpdateCityCommand>
{
    public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var city = await cityRepository.GetByIdAsync(request.Id, cancellationToken);

        if (city is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"City with id: {request.Id} does not exist.");
        }

        city.Update(request.Name, request.Country, request.FunFacts);
        await cityRepository.SaveChangesAsync(cancellationToken);
    }
}
