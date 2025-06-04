using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.AddTrain;

public sealed class AddTrainCommandHandler(IOperatorRepository operatorRepository)
    : ICommandHandler<AddTrainCommand, Guid>
{
    public async Task<Guid> Handle(AddTrainCommand request, CancellationToken cancellationToken)
    {
        var operatorEntity = await operatorRepository.FindAsync(request.OperatorId, cancellationToken);
        if (operatorEntity is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} not found.");
        }

        operatorEntity.AddTrain(request.Name, request.Coaches);
        await operatorRepository.SaveChangesAsync(cancellationToken);

        return operatorEntity.Trains.First(x => x.Name == request.Name).Id;
    }
}
