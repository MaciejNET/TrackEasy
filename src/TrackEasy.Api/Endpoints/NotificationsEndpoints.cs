using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackEasy.Application.Notifications.GetNotifications;
using TrackEasy.Application.Notifications.GetNotificationsCount;
using TrackEasy.Application.Notifications.DeleteNotification;
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

        group.MapGet("/count", async (ISender sender, CancellationToken ct) =>
            await sender.Send(new GetNotificationsCountQuery(), ct))
            .RequireAuthorization()
            .WithName("GetNotificationsCount")
            .Produces<int>()
            .WithDescription("Get notifications count for the current user.")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var command = new DeleteNotificationCommand(id);
            await sender.Send(command, ct);
            return Results.NoContent();
        })
            .RequireAuthorization()
            .WithName("DeleteNotification")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Mark a notification as read and remove it.")
            .WithOpenApi();
    }
}
