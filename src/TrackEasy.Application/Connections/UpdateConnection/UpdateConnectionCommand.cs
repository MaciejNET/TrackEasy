using TrackEasy.Application.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.UpdateConnection;

public sealed record UpdateConnectionCommand(Guid Id, string Name, MoneyDto Money) : ICommand;