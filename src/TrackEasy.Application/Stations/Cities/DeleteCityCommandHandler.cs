using MediatR;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Application.Stations.Cities;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand>
{
    private readonly ICityRepository _repository;

    public DeleteCityCommandHandler(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _repository.GetByIdAsync(request.Id);
        if (city == null) throw new Exception("City not found");

        _repository.Delete(city);
        await _repository.SaveChangesAsync();
    }
}
