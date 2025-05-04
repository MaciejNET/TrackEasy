using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.UpdateCoach;

internal sealed class UpdateCoachCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<UpdateCoachCommand>
{
    public Task Handle(UpdateCoachCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}