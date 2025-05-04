using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.DeleteCoach;

internal sealed class DeleteCoachCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<DeleteCoachCommand>
{
    public Task Handle(DeleteCoachCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}