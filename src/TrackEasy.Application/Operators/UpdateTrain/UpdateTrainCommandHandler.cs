using MediatR;
using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.UpdateTrain;

public sealed class UpdateTrainCommandHandler : IRequestHandler<UpdateTrainCommand>
{
    private readonly IOperatorRepository _operatorRepository;

    public UpdateTrainCommandHandler(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    public async Task<Unit> Handle(UpdateTrainCommand request, CancellationToken cancellationToken)
    {
        // 1. Znajd� operatora
        var operatorEntity = await _operatorRepository.FindAsync(request.OperatorId, cancellationToken);

        // 2. Je�li nie znaleziono � rzu� wyj�tek
        if (operatorEntity is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} not found.");
        }

        // 3. Zaktualizuj poci�g
        operatorEntity.UpdateTrain(request.TrainId, request.Name, request.Coaches);

        // 4. Zapisz zmiany
        await _operatorRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
