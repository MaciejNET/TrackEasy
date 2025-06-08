using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Application.Connections.CreateConnection;
using TrackEasy.Application.Connections.FindConnection;
using TrackEasy.Application.Connections.GetConnections;
using TrackEasy.Application.Operators.AddCoach;
using TrackEasy.Application.Operators.AddManager;
using TrackEasy.Application.Operators.AddTrain;
using TrackEasy.Application.Operators.CreateOperator;
using TrackEasy.Application.Operators.DeleteCoach;
using TrackEasy.Application.Operators.DeleteOperator;
using TrackEasy.Application.Operators.DeleteTrain;
using TrackEasy.Application.Operators.FindCoach;
using TrackEasy.Application.Operators.FindOperator;
using TrackEasy.Application.Operators.FindTrain;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Application.Operators.GetOperators;
using TrackEasy.Application.Operators.GetTrains;
using TrackEasy.Application.Operators.Shared;
using TrackEasy.Application.Operators.UpdateCoach;
using TrackEasy.Application.Operators.UpdateOperator;
using TrackEasy.Application.Operators.UpdateTrain;
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
                var query = new FindCoachQuery(coachId, id);
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

        group.MapGet("/{id:guid}/trains/{trainId:guid}",
            async (Guid id, Guid trainId, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new FindTrainQuery(id, trainId);
                var train = await sender.Send(query, cancellationToken);
                return train is null ? Results.NotFound() : Results.Ok(train);
            })
            .RequireManagerAccess()
            .WithName("FindOperatorTrain")
            .Produces<TrainDetailsDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Find a train by id for a specific operator.")
            .WithOpenApi();

        group.MapGet("/{id:guid}/trains", async (Guid id, [FromQuery] string? trainName, [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetTrainsQuery(id, trainName, pageNumber, pageSize);
            var trains = await sender.Send(query, cancellationToken);
            return Results.Ok(trains);
        })
        .RequireManagerAccess()
        .WithName("GetOperatorTrains")
        .Produces<PaginatedResult<TrainDto>>()
        .WithDescription("Get all trains for a specific operator.")
        .WithOpenApi();
        
        group.MapPost("/{id:guid}/trains",
            async (Guid id, AddTrainCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                command = command with { OperatorId = id };
                var trainId = await sender.Send(command, cancellationToken);
                return Results.Created($"/operators/{id}/trains/{trainId}", command);
            })
            .RequireManagerAccess()
            .WithName("CreateOperatorTrain")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a new train for an operator.")
            .WithOpenApi();
        
        group.MapPatch("/{id:guid}/trains/{trainId:guid}",
                async (Guid id, Guid trainId, UpdateTrainCommand command, ISender sender, CancellationToken cancellationToken) =>
                {
                    command = command with { TrainId = trainId, OperatorId = id };
                    await sender.Send(command, cancellationToken);
                    return Results.NoContent();
                })
            .RequireManagerAccess()
            .WithName("UpdateOperatorTrain")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update an existing train for a specific operator.")
            .WithOpenApi();
        
        group.MapDelete("/{id:guid}/trains/{trainId:guid}",
                async (Guid id, Guid trainId, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteTrainCommand(trainId, id);
                    await sender.Send(command, cancellationToken);
                    return Results.NoContent();
                })
            .RequireManagerAccess()
            .WithName("DeleteOperatorTrain")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Delete a train for a specific operator.")
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
        
        group.MapGet("/{id:guid}/connections", async (Guid id, [FromQuery] string? name, [FromQuery] string startStation, [FromQuery] string endStation,
                [FromQuery] int pageNumber, [FromQuery] int pageSize, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetConnectionsQuery(id, name, startStation, endStation, pageNumber, pageSize);
            var connections = await sender.Send(query, cancellationToken);
            return Results.Ok(connections);
        })
            .RequireManagerAccess()
            .WithName("GetOperatorConnections")
            .Produces<PaginatedResult<ConnectionDto>>()
            .WithDescription("Get all connections for a specific operator.")
            .WithOpenApi();
        
        group.MapGet("/{id:guid}/connections/{connectionId:guid}",
            async (Guid id, Guid connectionId, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new FindConnectionQuery(connectionId);
                var connection = await sender.Send(query, cancellationToken);
                return connection is null ? Results.NotFound() : Results.Ok(connection);
            })
            .RequireManagerAccess()
            .WithName("FindOperatorConnection")
            .Produces<ConnectionDetailsDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Find a connection by id for a specific operator.")
            .WithOpenApi();
        
        group.MapPost("/{id:guid}/connections", async (Guid id, CreateConnectionCommand command, ISender sender) =>
            {
                command = command with { OperatorId = id };
                await sender.Send(command);
                return Results.Ok();
            })
            .RequireManagerAccess()
            .WithName("CreateConnection")
            .WithSummary("Create a new connection")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a new connection between two stations with specified legs")
            .WithOpenApi();
    }
}