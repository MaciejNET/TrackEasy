using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.UpdateCoach;

internal sealed class UpdateCoachCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<UpdateCoachCommand>
{
    public async Task Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
    {
        var @operator = await operatorRepository.FindAsync(request.OperatorId, cancellationToken);
        if (@operator is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with id: {request.OperatorId} does not exist.");
        }

        @operator.UpdateCoach(request.Id, request.Name, request.SeatsNumbers);
        await operatorRepository.SaveChangesAsync(cancellationToken);
    }
}
