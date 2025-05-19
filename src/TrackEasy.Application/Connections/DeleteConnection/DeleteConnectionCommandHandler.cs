using TrackEasy.Domain.Connections;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.DeleteConnection;

internal sealed class DeleteConnectionCommandHandler(IConnectionRepository connectionRepository) : ICommandHandler<DeleteConnectionCommand>
{
    public Task Handle(DeleteConnectionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}