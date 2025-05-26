using MediatR;
using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.AddTrain;

public sealed class AddTrainCommandHandler : IRequestHandler<AddTrainCommand, Guid>
{
    private readonly IOperatorRepository _operatorRepository;

    public AddTrainCommandHandler(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    public async Task<Guid> Handle(AddTrainCommand request, CancellationToken cancellationToken)
    {
        
        var operatorEntity = await _operatorRepository.FindAsync(request.OperatorId, cancellationToken);

        
        if (operatorEntity is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with ID {request.OperatorId} not found.");
        }

       
        operatorEntity.AddTrain(request.Name, request.Coaches);

        
        await _operatorRepository.SaveChangesAsync(cancellationToken);

        
        var addedTrain = operatorEntity.Trains.First(t => t.Name == request.Name);
        return addedTrain.Id;
    }
}
