using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Application.Connections.ApproveConnectionRequest;
using TrackEasy.Application.Connections.RejectConnectionRequest;
using TrackEasy.Application.Connections.SearchConnections;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public class ConnectionEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/connections").WithTags("Connections");

        group.MapGet("/", async ([FromQuery] Guid startStationId,
                [FromQuery] Guid endStationId,
                [FromQuery] DateTime departureTime,
                ISender sender) =>
            {
                var query = new SearchConnectionsQuery(startStationId, endStationId, departureTime);
                return await sender.Send(query);
            })
            .WithName("SearchConnections")
            .WithSummary("Search for connections between two stations at a specific time")
            .Produces<PaginatedCursorResult<SearchConnectionsResponse>>()
            .WithDescription("Search for connections between two stations at a specific time")
            .WithOpenApi();
        
        group.MapPost("{id:guid}/approve", async (Guid id, ISender sender) =>
            {
                var command = new ApproveConnectionRequestCommand(id);
                await sender.Send(command);
                return Results.NoContent();
            })
            .RequireAdminAccess()
            .WithName("ApproveConnectionRequest")
            .WithSummary("Approve a connection request by ID")
            .Produces(StatusCodes.Status204NoContent)
            .WithDescription("Approve a connection request by ID")
            .WithOpenApi();
        
        group.MapPost("/{id:guid}/reject", async (Guid id, ISender sender) =>
            {
                var command = new RejectConnectionRequestCommand(id);
                await sender.Send(command);
                return Results.NoContent();
            })
            .RequireAdminAccess()
            .WithName("RejectConnectionRequest")
            .WithSummary("Reject a connection request by ID")
            .Produces(StatusCodes.Status204NoContent)
            .WithDescription("Reject a connection request by ID")
            .WithOpenApi();
    }
}