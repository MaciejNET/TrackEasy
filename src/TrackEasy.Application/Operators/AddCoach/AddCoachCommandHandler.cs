using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.AddCoach;

internal sealed class AddCoachCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<AddCoachCommand>
{
    public Task Handle(AddCoachCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}