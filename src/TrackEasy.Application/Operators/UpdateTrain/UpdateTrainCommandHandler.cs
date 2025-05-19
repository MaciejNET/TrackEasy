using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.UpdateTrain;

internal sealed class UpdateTrainCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<UpdateTrainCommand>
{
    public async Task Handle(UpdateTrainCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}