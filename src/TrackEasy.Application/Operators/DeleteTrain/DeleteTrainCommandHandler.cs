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
        // 1. ZnajdŸ operatora
        var operatorEntity = await _operatorRepository.FindAsync(request.OperatorId, cancellationToken);

        // 2. Jeœli brak — rzuæ wyj¹tek
        if (operatorEntity is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} not found.");
        }

        // 3. Usuñ poci¹g
        operatorEntity.DeleteTrain(request.TrainId);

        // 4. Zapisz zmiany
        await _operatorRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
