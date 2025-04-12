using FluentValidation;
using MediatR;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Application.Stations.Cities;

public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, Guid>
{
    private readonly ICityRepository _repository;
    private readonly IValidator<City> _validator;

    public CreateCityCommandHandler(ICityRepository repository, IValidator<City> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var city = new City
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CountryId = request.CountryId
        };

        await _validator.ValidateAndThrowAsync(city, cancellationToken);
        await _repository.AddAsync(city);
        await _repository.SaveChangesAsync();
        return city.Id;
    }
}
