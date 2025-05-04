using MediatR;
using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.DeleteOperator;
using TrackEasy.Application.Operators.FindOperator;
using TrackEasy.Application.Operators.GetOperators;
using TrackEasy.Application.Operators.Shared;
using TrackEasy.Application.Operators.UpdateOperator;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public class OperatorsEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/operators").WithTags("Operators");

        group.MapGet("/", async ([AsParameters] GetOperatorsQuery query, ISender sender, CancellationToken cancellationToken) 
                => await sender.Send(query, cancellationToken))
            .RequireAdminAccess()
            .WithName("GetOperators")
            .Produces<PaginatedResult<OperatorDto>>()
            .WithDescription("Get all operators.")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new FindOperatorQuery(id);
                var @operator = await sender.Send(query, cancellationToken);
                return @operator is null ? Results.NotFound() : Results.Ok(@operator);
            })
            .RequireAdminAccess()
            .WithName("FindOperator")
            .Produces<OperatorDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Find a operator by id.")
            .WithOpenApi();

        group.MapPost("/", async (CreateOperatorCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var id = await sender.Send(command, cancellationToken);
                return Results.Created($"/operators/{id}", command);
            })
            .RequireAdminAccess()
            .WithName("CreateOperator")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a new operator.")
            .WithOpenApi();
        
        group.MapPatch("/{id:guid}", async (Guid id, UpdateOperatorCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                command = command with { Id = id };
                await sender.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .RequireAdminAccess()
            .WithName("UpdateOperator")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update an existing operator.")
            .WithOpenApi();
        
        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new DeleteOperatorCommand(id);
                await sender.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .RequireAdminAccess()
            .WithName("DeleteOperator")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Delete a operator.")
            .WithOpenApi();
    }
}