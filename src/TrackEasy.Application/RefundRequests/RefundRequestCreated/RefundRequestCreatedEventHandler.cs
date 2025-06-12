using TrackEasy.Application.Services;
using TrackEasy.Domain.Managers;
using TrackEasy.Domain.Notifications;
using TrackEasy.Domain.RefundRequests;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.RefundRequests.RefundRequestCreated;

internal sealed class RefundRequestCreatedEventHandler(
    IManagerRepository managerRepository,
    INotificationRepository notificationRepository,
    INotificationService notificationService,
    TimeProvider timeProvider)
    : IDomainEventHandler<RefundRequestCreatedEvent>
{
    public async Task Handle(RefundRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        var managers = await managerRepository.GetByOperatorIdAsync(notification.OperatorId, cancellationToken);
        List<Task> notificationTasks = [];
        
        foreach (var manager in managers)
        {
            var requestNotification = Notification.Create(
                userId: manager.UserId,
                title: "Refund Request Created",
                message: "A new refund request has been created.",
                objectId: notification.Id,
                type: NotificationType.REFUND_REQUEST,
                timeProvider: timeProvider
            );

            notificationRepository.Add(requestNotification);

            notificationTasks.Add(notificationService.SendNotificationAsync(requestNotification));
        }

        await Task.WhenAll(notificationTasks);
        await notificationRepository.SaveChangesAsync(cancellationToken);
    }
}