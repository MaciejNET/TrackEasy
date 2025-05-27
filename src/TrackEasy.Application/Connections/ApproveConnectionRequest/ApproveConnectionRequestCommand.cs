using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.ApproveConnectionRequest;

public sealed record ApproveConnectionRequestCommand(Guid Id) : ICommand;