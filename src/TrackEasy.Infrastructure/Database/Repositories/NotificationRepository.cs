using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Notifications;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class NotificationRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), INotificationRepository
{
    private readonly TrackEasyDbContext _dbContext = dbContext;

    public async Task<Notification?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

    public void Add(Notification notification)
    {
        _dbContext.Notifications.Add(notification);
    }

    public void Delete(Notification notification)
    {
        _dbContext.Notifications.Remove(notification);
    }
}
