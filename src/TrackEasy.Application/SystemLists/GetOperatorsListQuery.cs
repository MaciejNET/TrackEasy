using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.SystemLists;

public sealed record GetOperatorsListQuery : IQuery<IEnumerable<SystemListItemDto>>;