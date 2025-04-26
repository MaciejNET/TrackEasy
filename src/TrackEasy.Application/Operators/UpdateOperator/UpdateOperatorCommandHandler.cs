using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.UpdateOperator;

internal sealed class UpdateOperatorCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<UpdateOperatorCommand>
{
    public async Task Handle(UpdateOperatorCommand request, CancellationToken cancellationToken)
    {
        var @operator = await operatorRepository.FindAsync(request.Id, cancellationToken);

        if (@operator is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound,
                $"Operator with id: {request.Id} does not exists.");
        }

        var exists = await operatorRepository.ExistsAsync(request.Id, request.Name, request.Code, cancellationToken);
        
        if (exists)
        {
            throw new TrackEasyException(Codes.OperatorAlreadyExists,
                $"Operator with name: {request.Name} or code: {request.Code} already exists.");
        }
        
        @operator.Update(request.Name, request.Code);
        await operatorRepository.SaveChangesAsync(cancellationToken);
    }
}