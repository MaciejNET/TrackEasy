using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.DeleteTrain;

internal sealed class DeleteTrainCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<DeleteTrainCommand>
{
    public async Task Handle(DeleteTrainCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}