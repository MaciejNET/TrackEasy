using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Notifications.GetNotifications;
using TrackEasy.Application.Security;
using TrackEasy.Domain.Notifications;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Notifications;

internal sealed class GetNotificationsQueryHandler(
    TrackEasyDbContext dbContext,
    IUserContext userContext)
    : IQueryHandler<GetNotificationsQuery, PaginatedResult<NotificationDto>>
{
    public async Task<PaginatedResult<NotificationDto>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = userContext.UserId ?? throw new InvalidOperationException("User is not authenticated.");

        return await dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationDto(
                n.Id,
                n.Title,
                n.Message,
                n.Type,
                n.ObjectId,
                n.CreatedAt))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
