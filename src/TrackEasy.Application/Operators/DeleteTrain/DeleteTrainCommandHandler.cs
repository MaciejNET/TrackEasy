using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.DeleteTrain;

public sealed class DeleteTrainCommandHandler(IOperatorRepository operatorRepository)
    : ICommandHandler<DeleteTrainCommand>
{
    public async Task Handle(DeleteTrainCommand request, CancellationToken cancellationToken)
    {
        var operatorEntity = await operatorRepository.FindAsync(request.OperatorId, cancellationToken);
        if (operatorEntity is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} not found.");
        }

        operatorEntity.DeleteTrain(request.TrainId);
        await operatorRepository.SaveChangesAsync(cancellationToken);
    }
}
