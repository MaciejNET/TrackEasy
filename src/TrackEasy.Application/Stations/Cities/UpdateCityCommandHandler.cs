using FluentValidation;
using MediatR;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Application.Stations.Cities;

public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand>
{
    private readonly ICityRepository _repository;
    private readonly IValidator<City> _validator;

    public UpdateCityCommandHandler(ICityRepository repository, IValidator<City> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _repository.GetByIdAsync(request.Id);
        if (city == null) throw new Exception("City not found");

        city.Name = request.Name;
        city.CountryId = request.CountryId;

        await _validator.ValidateAndThrowAsync(city, cancellationToken);
        _repository.Update(city);
        await _repository.SaveChangesAsync();
    }
}
