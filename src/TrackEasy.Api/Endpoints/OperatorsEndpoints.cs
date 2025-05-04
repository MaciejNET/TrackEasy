using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Application.Operators.AddCoach;
using TrackEasy.Application.Operators.AddManager;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.DeleteCoach;
using TrackEasy.Application.Operators.DeleteOperator;
using TrackEasy.Application.Operators.FindCoach;
using TrackEasy.Application.Operators.FindOperator;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Application.Operators.GetOperators;
using TrackEasy.Application.Operators.Shared;
using TrackEasy.Application.Operators.UpdateCoach;
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
        
        group.MapGet("/{id:guid}/coaches", async (Guid id, [FromQuery] string? code, [FromQuery] int pageNumber, [FromQuery] int pageSize,
                ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetCoachesQuery(id, code, pageNumber, pageSize);
            var coaches = await sender.Send(query, cancellationToken);
            return Results.Ok(coaches);
        })
            .RequireManagerAccess()
            .WithName("GetOperatorCoaches")
            .Produces<PaginatedResult<CoachDto>>()
            .WithDescription("Get all coaches for a specific operator.")
            .WithOpenApi();

        group.MapGet("/{id:guid}/coaches/{coachId:guid}",
            async (Guid id, Guid coachId, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new FindCoachQuery(id, coachId);
                var coach = await sender.Send(query, cancellationToken);
                return coach is null ? Results.NotFound() : Results.Ok(coach);
            })
            .RequireManagerAccess()
            .WithName("FindOperatorCoach")
            .Produces<CoachDetailsDto>()
            .WithDescription("Find a coach by id.")
            .WithOpenApi();

        group.MapPost("/{id:guid}/coaches",
            async (Guid id, AddCoachCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                command = command with { OperatorId = id };
                await sender.Send(command, cancellationToken);
                return Results.Ok();
            })
            .RequireManagerAccess()
            .WithName("AddOperatorCoach")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Add a new coach to an operator.")
            .WithOpenApi();
        
        group.MapPatch("/{id:guid}/coaches/{coachId:guid}",
                async (Guid id, Guid coachId, UpdateCoachCommand command, ISender sender, CancellationToken cancellationToken) =>
                {
                    command = command with { Id = coachId, OperatorId = id };
                    await sender.Send(command, cancellationToken);
                    return Results.NoContent();
                })
            .RequireManagerAccess()
            .WithName("UpdateOperatorCoach")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update a coach for a specific operator.")
            .WithOpenApi();

        group.MapDelete("/{id:guid}/coaches/{coachId:guid}",
                async (Guid id, Guid coachId, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteCoachCommand(coachId, id);
                    await sender.Send(command, cancellationToken);
                    return Results.NoContent();
                })
            .RequireManagerAccess()
            .WithName("DeleteOperatorCoach")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Delete a coach for a specific operator.")
            .WithOpenApi();

        group.MapPost("/{id:guid}/managers",
                async (Guid id, AddManagerCommand command, ISender sender, CancellationToken cancellationToken) =>
                {
                    command = command with { OperatorId = id };
                    await sender.Send(command, cancellationToken);
                    return Results.Ok();
                })
            .RequireAdminAccess()
            .WithName("AddOperatorManager")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Add a new manager to an operator.")
            .WithOpenApi();
    }
}