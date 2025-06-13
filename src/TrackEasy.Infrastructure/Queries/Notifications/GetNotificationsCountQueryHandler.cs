using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Notifications.GetNotificationsCount;
using TrackEasy.Application.Security;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Notifications;

internal sealed class GetNotificationsCountQueryHandler(
    TrackEasyDbContext dbContext,
    IUserContext userContext)
    : IQueryHandler<GetNotificationsCountQuery, int>
{
    public async Task<int> Handle(GetNotificationsCountQuery request, CancellationToken cancellationToken)
    {
        var userId = userContext.UserId ?? throw new InvalidOperationException("User is not authenticated.");

        return await dbContext.Notifications
            .AsNoTracking()
            .CountAsync(n => n.UserId == userId, cancellationToken);
    }
}
