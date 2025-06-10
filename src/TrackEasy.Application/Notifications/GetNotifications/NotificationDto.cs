namespace TrackEasy.Application.Notifications.GetNotifications;

using TrackEasy.Domain.Notifications;

public sealed record NotificationDto(
    Guid Id,
    string Title,
    string Message,
    NotificationType Type,
    Guid ObjectId,
    DateTime CreatedAt);
