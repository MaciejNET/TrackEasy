using MediatR;
using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.DeleteTrain;

public sealed class DeleteTrainCommandHandler : IRequestHandler<DeleteTrainCommand>
{
    private readonly IOperatorRepository _operatorRepository;

    public DeleteTrainCommandHandler(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    public async Task<Unit> Handle(DeleteTrainCommand request, CancellationToken cancellationToken)
    {
        // 1. Znajd� operatora
        var operatorEntity = await _operatorRepository.FindAsync(request.OperatorId, cancellationToken);

        // 2. Je�li brak � rzu� wyj�tek
        if (operatorEntity is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} not found.");
        }

        // 3. Usu� poci�g
        operatorEntity.DeleteTrain(request.TrainId);

        // 4. Zapisz zmiany
        await _operatorRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
