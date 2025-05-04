using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.FindStation;

public sealed record FindStationQuery(Guid Id) : IQuery<StationDetailsDto?>;