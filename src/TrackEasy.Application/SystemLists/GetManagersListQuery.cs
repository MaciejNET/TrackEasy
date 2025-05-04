using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.SystemLists;

public sealed record GetManagersListQuery(Guid OperatorId) : IQuery<IEnumerable<SystemListItemDto>>;