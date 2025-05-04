using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.SystemLists;

public sealed record GetDiscountsListQuery : IQuery<IEnumerable<SystemListItemDto>>;