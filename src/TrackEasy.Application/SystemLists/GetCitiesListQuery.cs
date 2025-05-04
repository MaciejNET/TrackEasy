using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.SystemLists;

public sealed record GetCitiesListQuery : IQuery<IEnumerable<SystemListItemDto>>;