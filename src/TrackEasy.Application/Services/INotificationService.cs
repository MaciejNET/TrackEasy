using TrackEasy.Domain.Notifications;

namespace TrackEasy.Application.Services;

public interface INotificationService
{
    Task SendNotificationAsync(Notification notification);
}