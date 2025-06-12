using Microsoft.AspNetCore.Identity;
using TrackEasy.Application.Services;
using TrackEasy.Domain.Connections;
using TrackEasy.Domain.Notifications;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.ConnectionRequestCreated;

internal sealed class ConnectionRequestCreatedEventHandler(
    INotificationRepository notificationRepository,
    INotificationService notificationService,
    UserManager<User> userManager,
    TimeProvider timeProvider)
    : IDomainEventHandler<ConnectionRequestCreatedEvent>
{
    public async Task Handle(ConnectionRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        var admins = await userManager.GetUsersInRoleAsync(Roles.Admin);
        var userIds = admins.Select(admin => admin.Id).ToList();
        List<Task> notificationTasks = [];

        foreach (var userId in userIds)
        {
            var requestNotification = Notification.Create(
                userId: userId,
                title: "New Connection Request",
                message: $"A new connection request has been created by {notification.OperatorName} for {notification.Name}.",
                objectId: notification.ConnectionId,
                type: NotificationType.CONNECTION_REQUEST,
                timeProvider: timeProvider
            );

            notificationRepository.Add(requestNotification);

            notificationTasks.Add(notificationService.SendNotificationAsync(requestNotification));
        }

        await Task.WhenAll(notificationTasks);
        await notificationRepository.SaveChangesAsync(cancellationToken);
    }
}