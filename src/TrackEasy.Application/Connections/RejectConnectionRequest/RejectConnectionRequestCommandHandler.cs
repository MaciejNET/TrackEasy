using TrackEasy.Domain.Connections;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.RejectConnectionRequest;

internal sealed class RejectConnectionRequestCommandHandler(IConnectionRepository connectionRepository) : ICommandHandler<RejectConnectionRequestCommand>
{
    public Task Handle(RejectConnectionRequestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}