using MediatR;
using TrackEasy.Application.DiscountCodes.Shared;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.DeleteOperator;
using TrackEasy.Application.Operators.FindOperator;
using TrackEasy.Application.Operators.GetOperators;
using TrackEasy.Application.Operators.Shared;
using TrackEasy.Application.Operators.UpdateOperator;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public static class OperatorsEndpoints
{
    public static void MapOperatorsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/operators").WithTags("Operators");

        group.MapGet("/", async ([AsParameters] GetOperatorsQuery query, ISender sender, CancellationToken cancellationToken) 
                => await sender.Send(query, cancellationToken))
            .WithName("GetOperators")
            .Produces<PaginatedResult<DiscountCodeDto>>()
            .WithDescription("Get all operators.")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new FindOperatorQuery(id);
                var @operator = await sender.Send(query, cancellationToken);
                return @operator is null ? Results.NotFound() : Results.Ok(@operator);
            })
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
            .WithName("DeleteOperator")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Delete a operator.")
            .WithOpenApi();
    }
}