using TrackEasy.Domain.Connections;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Connections.RejectConnectionRequest;

internal sealed class RejectConnectionRequestCommandHandler(
    IConnectionRepository connectionRepository
) : ICommandHandler<RejectConnectionRequestCommand>
{
    public async Task Handle(RejectConnectionRequestCommand request, CancellationToken cancellationToken)
    {
        var connection = await connectionRepository.FindByIdAsync(request.Id, cancellationToken);

        if (connection is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Connection with ID {request.Id} was not found.");
        }

        connection.RejectRequest();

        await connectionRepository.SaveChangesAsync(cancellationToken);
    }
}