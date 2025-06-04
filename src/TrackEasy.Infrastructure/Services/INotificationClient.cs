using TrackEasy.Domain.Notifications;

namespace TrackEasy.Infrastructure.Services;

public interface INotificationClient
{
    Task ReceiveNotificationAsync(string title, string message, Guid objectId, NotificationType notificationType,
        DateTime createdAt);
}