using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.AddCoach;

internal sealed class AddCoachCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<AddCoachCommand>
{
    public async Task Handle(AddCoachCommand request, CancellationToken cancellationToken)
    {
        var @operator = await operatorRepository.FindAsync(request.OperatorId, cancellationToken);
        if (@operator is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with id: {request.OperatorId} does not exists.");
        }

        @operator.AddCoach(request.Code, request.SeatsNumbers);
        await operatorRepository.SaveChangesAsync(cancellationToken);
    }
}
