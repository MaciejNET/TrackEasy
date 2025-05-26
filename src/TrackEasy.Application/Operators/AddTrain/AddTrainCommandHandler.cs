using MediatR;
using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.AddTrain;

public sealed class AddTrainCommandHandler : IRequestHandler<AddTrainCommand, Guid>
{
    private readonly IOperatorRepository _operatorRepository;

    public AddTrainCommandHandler(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    public async Task<Guid> Handle(AddTrainCommand request, CancellationToken cancellationToken)
    {
        // 1. Znajd� operatora
        var operatorEntity = await _operatorRepository.FindAsync(request.OperatorId, cancellationToken);

        // 2. Je�li nie znaleziono � rzu� wyj�tek
        if (operatorEntity is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} not found.");
        }

        // 3. Dodaj poci�g
        operatorEntity.AddTrain(request.Name, request.Coaches);

        // 4. (opcjonalne) _operatorRepository.Add(operatorEntity); � ale raczej niepotrzebne przy edycji istniej�cego

        // 5. Zapisz zmiany
        await _operatorRepository.SaveChangesAsync(cancellationToken);

        // 6. Zwr�� ID nowego poci�gu
        var addedTrain = operatorEntity.Trains.First(t => t.Name == request.Name);
        return addedTrain.Id;
    }
}
