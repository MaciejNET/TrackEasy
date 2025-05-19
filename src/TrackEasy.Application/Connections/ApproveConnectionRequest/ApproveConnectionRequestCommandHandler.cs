using TrackEasy.Domain.Connections;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.ApproveConnectionRequest;

internal sealed class ApproveConnectionRequestCommandHandler(IConnectionRepository connectionRepository) : ICommandHandler<ApproveConnectionRequestCommand>
{
    public Task Handle(ApproveConnectionRequestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}