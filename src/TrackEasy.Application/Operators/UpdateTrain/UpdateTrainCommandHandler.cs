using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.UpdateTrain;

public sealed class UpdateTrainCommandHandler(IOperatorRepository operatorRepository)
    : ICommandHandler<UpdateTrainCommand>
{
    public async Task Handle(UpdateTrainCommand request, CancellationToken cancellationToken)
    {
        var operatorEntity = await operatorRepository.FindAsync(request.OperatorId, cancellationToken);
        if (operatorEntity is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} not found.");
        }

        operatorEntity.UpdateTrain(request.TrainId, request.Name, request.Coaches);
        await operatorRepository.SaveChangesAsync(cancellationToken);
    }
}
