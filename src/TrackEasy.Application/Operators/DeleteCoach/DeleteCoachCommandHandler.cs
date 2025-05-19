using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.DeleteCoach;

internal sealed class DeleteCoachCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<DeleteCoachCommand>
{
    public async Task Handle(DeleteCoachCommand request, CancellationToken cancellationToken)
    {
        var @operator = await operatorRepository.FindAsync(request.OperatorId, cancellationToken);
        if (@operator is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with id: {request.OperatorId} does not exists.");
        }

        @operator.DeleteCoach(request.Id);
        await operatorRepository.SaveChangesAsync(cancellationToken);
    }
}
