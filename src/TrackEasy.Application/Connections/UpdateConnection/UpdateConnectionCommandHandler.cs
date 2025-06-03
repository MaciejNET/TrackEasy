using TrackEasy.Domain.Connections;
using TrackEasy.Domain.Shared;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Connections.UpdateConnection;

internal sealed class UpdateConnectionCommandHandler(IConnectionRepository connectionRepository) : ICommandHandler<UpdateConnectionCommand>
{
    public async Task Handle(UpdateConnectionCommand request, CancellationToken cancellationToken)
    {
        var connection = await connectionRepository.FindByIdAsync(request.Id, cancellationToken);

        if (connection is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Connection with ID {request.Id} was not found.");
        }

        connection.Update(request.Name, new Money(request.Money.Amount, request.Money.Currency));
        
        await connectionRepository.SaveChangesAsync(cancellationToken);
    }
}