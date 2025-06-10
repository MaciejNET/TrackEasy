using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackEasy.Application.Notifications.GetNotifications;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public class NotificationsEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/notifications").WithTags("Notifications");

        group.MapGet("/", async ([AsParameters] GetNotificationsQuery query, ISender sender, CancellationToken ct) =>
                await sender.Send(query, ct))
            .RequireAuthorization()
            .WithName("GetNotifications")
            .Produces<PaginatedResult<NotificationDto>>()
            .WithDescription("Get paginated notifications for the current user.")
            .WithOpenApi();
    }
}
