using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.FindConnection;

public sealed record FindConnectionQuery(Guid Id) : IQuery<ConnectionDetailsDto?>;