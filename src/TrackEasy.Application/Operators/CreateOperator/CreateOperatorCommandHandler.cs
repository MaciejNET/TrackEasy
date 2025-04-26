using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.CreateOperator;

internal sealed class CreateOperatorCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<CreateOperatorCommand, Guid>
{
    public async Task<Guid> Handle(CreateOperatorCommand request, CancellationToken cancellationToken)
    {
        var exists = await operatorRepository.ExistsAsync(request.Name, request.Code, cancellationToken);

        if (exists)
        {
            throw new TrackEasyException(Codes.OperatorAlreadyExists,
                $"Operator with name: {request.Name} or code: {request.Code} already exists.");
        }
        
        var @operator = Operator.Create(request.Name, request.Code);
        
        operatorRepository.Add(@operator);
        await operatorRepository.SaveChangesAsync(cancellationToken);
        
        return @operator.Id;
    }
}