using TrackEasy.Application.Services;
using TrackEasy.Domain.Connections;
using TrackEasy.Domain.Managers;
using TrackEasy.Domain.Notifications;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.ConnectionRequestApproved;

internal sealed class ConnectionRequestApprovedEventHandler(IManagerRepository managerRepository, INotificationService notificationService, TimeProvider timeProvider)
    : IDomainEventHandler<ConnectionRequestApprovedEvent>
{
    public async Task Handle(ConnectionRequestApprovedEvent notification, CancellationToken cancellationToken)
    {
         var mangers = await managerRepository.GetByOperatorIdAsync(notification.OperatorId, cancellationToken);
         List<Task> notificationTasks = [];
         
         foreach (var manager in mangers)
         {
             var requestNotification = Notification.Create(
                 userId: manager.UserId,
                 title: "Connection Request Approved",
                 message: GenerateMessage(notification.RequestType, notification.Name),
                 objectId: notification.ConnectionId,
                 type: NotificationType.CONNECTION_REQUEST,
                 timeProvider: timeProvider
             );
             
             notificationTasks.Add(notificationService.SendNotificationAsync(requestNotification));
         }
         
         await Task.WhenAll(notificationTasks);
    }

    private static string GenerateMessage(ConnectionRequestType type, string name)
    {
        return type switch
        {
            ConnectionRequestType.ADD => $"Your connection request for {name} has been approved.",
            ConnectionRequestType.UPDATE => $"Your connection update request for name {name} has been approved.",
            ConnectionRequestType.DELETE => $"Your connection deletion request for {name} has been approved.",
            _ => "Your connection request has been approved."
        };
    }
}