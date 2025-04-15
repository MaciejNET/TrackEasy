using MediatR;
using TrackEasy.Application.Stations.CreateCity;
using TrackEasy.Application.Stations.DeleteCity;
using TrackEasy.Application.Stations.FindCity;
using TrackEasy.Application.Stations.GetCities;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Application.Stations.UpdateCity;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public static class CitiesEndpoints
{
    public static void MapCitiesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/cities").WithTags("Cities");

        group.MapGet("/", async ([AsParameters] GetCitiesQuery query, ISender sender, CancellationToken cancellationToken) =>
            await sender.Send(query, cancellationToken))
            .WithName("GetCities")
            .Produces<PaginatedResult<CityDto>>()
            .WithDescription("Get all cities.")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var city = await sender.Send(new FindCityQuery(id), cancellationToken);
                return city is null ? Results.NotFound() : Results.Ok(city);
            })
            .WithName("FindCity")
            .Produces<CityDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Find a city by ID.")
            .WithOpenApi();

        group.MapPost("/", async (CreateCityCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(command, cancellationToken);
                return Results.Created("/cities", command);
            })
            .WithName("CreateCity")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a new city.")
            .WithOpenApi();

        group.MapPatch("/{id:guid}", async (Guid id, UpdateCityCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                command = command with { Id = id };
                await sender.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .WithName("UpdateCity")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update an existing city.")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new DeleteCityCommand(id);
                await sender.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .WithName("DeleteCity")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Delete a city.")
            .WithOpenApi();
    }
}
