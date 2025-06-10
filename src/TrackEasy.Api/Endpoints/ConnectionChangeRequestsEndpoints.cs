using MediatR;
using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Application.Connections.FindConnectionChangeRequest;
using TrackEasy.Application.Connections.GetConnectionChangeRequests;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public class ConnectionChangeRequestsEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/connection-change-requests").WithTags("Connection Change Requests");

        group.MapGet("/", async ([AsParameters] GetConnectionChangeRequestsQuery query, ISender sender, CancellationToken ct) =>
                await sender.Send(query, ct))
            .RequireAdminAccess()
            .WithName("GetConnectionChangeRequests")
            .WithSummary("Get paginated connection change requests")
            .Produces<PaginatedResult<ConnectionChangeRequestDto>>()
            .WithDescription("Get paginated list of connection change requests")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new FindConnectionChangeRequestQuery(id), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
            .RequireAdminAccess()
            .WithName("FindConnectionChangeRequest")
            .WithSummary("Find connection change request by connection ID")
            .Produces<ConnectionChangeRequestDetailsDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Get full details of a connection change request")
            .WithOpenApi();
    }
}
