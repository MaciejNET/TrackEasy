using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Notifications;

public interface INotificationRepository : IBaseRepository
{
    Task<Notification?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    void Add(Notification notification);
    void Delete(Notification notification);
}
