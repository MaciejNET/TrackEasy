using MediatR;
using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.UpdateTrain;

public sealed class UpdateTrainCommandHandler : IRequestHandler<UpdateTrainCommand, Unit>
{
    private readonly IOperatorRepository _operatorRepository;

    public UpdateTrainCommandHandler(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    public async Task<Unit> Handle(UpdateTrainCommand request, CancellationToken cancellationToken)
    {
        var operatorEntity = await _operatorRepository.FindAsync(request.OperatorId, cancellationToken);

        if (operatorEntity is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} not found.");
        }

        operatorEntity.UpdateTrain(request.TrainId, request.Name, request.Coaches);

        await _operatorRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
