using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.SystemLists;

public sealed record GetStationsListQuery : IQuery<IEnumerable<SystemListItemDto>>;