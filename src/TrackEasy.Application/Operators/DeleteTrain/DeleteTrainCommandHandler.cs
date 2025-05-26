using MediatR;
using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.DeleteTrain;

public sealed class DeleteTrainCommandHandler : IRequestHandler<DeleteTrainCommand, Unit>
{
    private readonly IOperatorRepository _operatorRepository;

    public DeleteTrainCommandHandler(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    public async Task<Unit> Handle(DeleteTrainCommand request, CancellationToken cancellationToken)
    {
        var operatorEntity = await _operatorRepository.FindAsync(request.OperatorId, cancellationToken);

        if (operatorEntity is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} not found.");
        }

        operatorEntity.DeleteTrain(request.TrainId);

        await _operatorRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
