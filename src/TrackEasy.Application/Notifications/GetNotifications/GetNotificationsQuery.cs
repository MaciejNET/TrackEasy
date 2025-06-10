using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Notifications.GetNotifications;

public sealed record GetNotificationsQuery(int PageNumber, int PageSize)
    : IQuery<PaginatedResult<NotificationDto>>;
