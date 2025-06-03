using Microsoft.AspNetCore.SignalR;
using TrackEasy.Application.Services;
using TrackEasy.Domain.Notifications;

namespace TrackEasy.Infrastructure.Services;

internal sealed class NotificationService(IHubContext<NotificationHub, INotificationClient> notificationHub) : INotificationService
{
    public async Task SendNotificationAsync(Notification notification)
    {
        await notificationHub.Clients.User(notification.UserId.ToString())
            .ReceiveNotificationAsync(notification.Title, notification.Message, 
                notification.ObjectId, notification.Type, notification.CreatedAt);
    }
}