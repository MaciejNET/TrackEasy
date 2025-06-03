using MediatR;
using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Application.Cities.CreateCity;
using TrackEasy.Application.Cities.DeleteCity;
using TrackEasy.Application.Cities.FindCity;
using TrackEasy.Application.Cities.GetCities;
using TrackEasy.Application.Cities.GetCountries;
using TrackEasy.Application.Cities.UpdateCity;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public class CitiesEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/cities").WithTags("Cities");

        group.MapGet("/countries", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetCountriesQuery();
                var countries = await sender.Send(query, cancellationToken);
                return Results.Ok(countries);
            })
            .RequireAdminAccess()
            .WithName("GetCountries")
            .Produces<IEnumerable<CountryDto>>()
            .WithDescription("Get all countries.")
            .WithOpenApi();
        
        group.MapGet("/", async ([AsParameters] GetCitiesQuery query, ISender sender, CancellationToken cancellationToken) =>
            await sender.Send(query, cancellationToken))
            .RequireAdminAccess()
            .WithName("GetCities")
            .Produces<PaginatedResult<CityDto>>()
            .WithDescription("Get all cities.")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var city = await sender.Send(new FindCityQuery(id), cancellationToken);
                return city is null ? Results.NotFound() : Results.Ok(city);
            })
            .RequireAdminAccess()
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
            .RequireAdminAccess()
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
            .RequireAdminAccess()
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
            .RequireAdminAccess()
            .WithName("DeleteCity")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Delete a city.")
            .WithOpenApi();
    }
}
