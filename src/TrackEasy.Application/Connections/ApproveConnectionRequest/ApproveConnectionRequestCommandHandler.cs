using TrackEasy.Domain.Connections;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Connections.ApproveConnectionRequest;

internal sealed class ApproveConnectionRequestCommandHandler(
    IConnectionRepository connectionRepository
) : ICommandHandler<ApproveConnectionRequestCommand>
{
    public async Task Handle(ApproveConnectionRequestCommand request, CancellationToken cancellationToken)
    {
        var connection = await connectionRepository.FindByIdAsync(request.Id, cancellationToken);

        if (connection is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Connection with ID {request.Id} was not found.");
        }

        connection.ApproveRequest();

        await connectionRepository.SaveChangesAsync(cancellationToken);
    }
}