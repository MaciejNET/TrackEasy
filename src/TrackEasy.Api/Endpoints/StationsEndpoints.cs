using MediatR;
using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Application.Stations;
using TrackEasy.Application.Stations.CreateStation;
using TrackEasy.Application.Stations.DeleteStation;
using TrackEasy.Application.Stations.FindStation;
using TrackEasy.Application.Stations.GetStations;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Application.Stations.UpdateStation;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public class StationsEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/stations").WithTags("Stations");
        
        group.MapGet("/", async ([AsParameters] GetStationsQuery query, ISender sender, CancellationToken cancellationToken) =>
            await sender.Send(query, cancellationToken))
        .RequireAdminAccess()
        .WithName("GetStations")
        .Produces<PaginatedResult<StationDto>>()
        .WithDescription("Get all stations.")
        .WithOpenApi();

    group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new FindStationQuery(id);
            var station = await sender.Send(query, cancellationToken);
            return station is null ? Results.NotFound() : Results.Ok(station);
        })
        .RequireAdminAccess()
        .WithName("FindStation")
        .Produces<StationDetailsDto>()
        .Produces(StatusCodes.Status404NotFound)
        .WithDescription("Find a station by id.")
        .WithOpenApi();
    
    group.MapGet("/nearest", async ([AsParameters] GeographicalCoordinatesDto geographicalCoordinates, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetNearestStationQuery(geographicalCoordinates);
            var station = await sender.Send(query, cancellationToken);
            return station is null ? Results.NotFound() : Results.Ok(station);
        })
        .WithName("GetNearestStation")
        .Produces<StationDto>()
        .Produces(StatusCodes.Status404NotFound)
        .WithDescription("Get the nearest station based on geographical coordinates.")
        .WithOpenApi();

    group.MapPost("/", async (CreateStationCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var id = await sender.Send(command, cancellationToken);
            return Results.Created($"/stations/{id}", command);
        })
        .RequireAdminAccess()
        .WithName("CreateStation")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithDescription("Create a new station.")
        .WithOpenApi();

    group.MapPatch("/{id:guid}", async (Guid id, UpdateStationCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            command = command with { Id = id };
            await sender.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .RequireAdminAccess()
        .WithName("UpdateStation")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .WithDescription("Update an existing station.")
        .WithOpenApi();

    group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteStationCommand(id);
            await sender.Send(command, cancellationToken);
            return Results.NoContent();
        })
        .RequireAdminAccess()
        .WithName("DeleteStation")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .WithDescription("Delete a station.")
        .WithOpenApi();
    }
}