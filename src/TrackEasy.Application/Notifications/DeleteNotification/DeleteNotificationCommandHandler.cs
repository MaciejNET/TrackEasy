using TrackEasy.Domain.Notifications;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Notifications.DeleteNotification;

internal sealed class DeleteNotificationCommandHandler(INotificationRepository notificationRepository) : ICommandHandler<DeleteNotificationCommand>
{
    public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.FindByIdAsync(request.Id, cancellationToken);

        if (notification is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Notification with id {request.Id} was not found.");
        }

        notificationRepository.Delete(notification);
        await notificationRepository.SaveChangesAsync(cancellationToken);
    }
}
