using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.AddTrain;

internal sealed class AddTrainCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<AddTrainCommand, Guid>
{
    public async Task<Guid> Handle(AddTrainCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}