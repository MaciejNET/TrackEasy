using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.SystemLists;

public sealed record GetAdminsListQuery : IQuery<IEnumerable<SystemListItemDto>>;