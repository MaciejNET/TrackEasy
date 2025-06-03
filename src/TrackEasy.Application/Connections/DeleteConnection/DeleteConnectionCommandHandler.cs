using TrackEasy.Domain.Connections;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Connections.DeleteConnection;

internal sealed class DeleteConnectionCommandHandler(
    IConnectionRepository connectionRepository
) : ICommandHandler<DeleteConnectionCommand>
{
    public async Task Handle(DeleteConnectionCommand request, CancellationToken cancellationToken)
    {
        var connection = await connectionRepository.FindByIdAsync(request.Id, cancellationToken);

        if (connection is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Connection with ID {request.Id} was not found.");
        }

        connection.Delete();

        await connectionRepository.SaveChangesAsync(cancellationToken);
    }
}