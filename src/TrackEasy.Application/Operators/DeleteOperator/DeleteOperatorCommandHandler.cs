using TrackEasy.Domain.Operators;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.DeleteOperator;

internal sealed class DeleteOperatorCommandHandler(IOperatorRepository operatorRepository) : ICommandHandler<DeleteOperatorCommand>
{
    public async Task Handle(DeleteOperatorCommand request, CancellationToken cancellationToken)
    {
        var @operator = await operatorRepository.FindAsync(request.Id, cancellationToken);

        if (@operator is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with id: {request.Id} does not exits.");
        }
        
        operatorRepository.Delete(@operator);
        await operatorRepository.SaveChangesAsync(cancellationToken);
    }
}