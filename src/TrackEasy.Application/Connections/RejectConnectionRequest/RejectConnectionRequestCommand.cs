using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.RejectConnectionRequest;

public sealed record RejectConnectionRequestCommand(Guid Id) : ICommand;