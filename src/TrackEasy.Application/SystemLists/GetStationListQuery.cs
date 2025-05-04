using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.SystemLists;

public sealed record GetStationListQuery : IQuery<IEnumerable<SystemListItemDto>>;