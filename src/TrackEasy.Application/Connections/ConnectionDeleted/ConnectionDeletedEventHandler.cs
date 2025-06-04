using TrackEasy.Domain.Connections;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.ConnectionDeleted;

internal sealed class ConnectionDeletedEventHandler(IConnectionRepository connectionRepository) : IDomainEventHandler<ConnectionDeletedEvent>
{
    public async Task Handle(ConnectionDeletedEvent notification, CancellationToken cancellationToken)
    {
        var connection = await connectionRepository.FindByIdAsync(notification.ConnectionId, cancellationToken);
        
        if (connection is null)
        {
            return;
        }
        
        connectionRepository.Delete(connection);
        await connectionRepository.SaveChangesAsync(cancellationToken);
    }
}