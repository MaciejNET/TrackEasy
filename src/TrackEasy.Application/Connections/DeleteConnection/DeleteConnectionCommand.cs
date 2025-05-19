using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.DeleteConnection;

public sealed record DeleteConnectionCommand(Guid Id) : ICommand;