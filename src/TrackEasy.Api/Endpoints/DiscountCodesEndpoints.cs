using MediatR;
using TrackEasy.Application.DiscountCodes.CreateDiscountCode;
using TrackEasy.Application.DiscountCodes.DeleteDiscountCode;
using TrackEasy.Application.DiscountCodes.FindDiscountCode;
using TrackEasy.Application.DiscountCodes.GetDiscountCodes;
using TrackEasy.Application.DiscountCodes.Shared;
using TrackEasy.Application.DiscountCodes.UpdateDiscountCode;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public static class DiscountCodesEndpoints
{
    public static void MapDiscountCodesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/discount-codes").WithTags("Discount Codes");

        group.MapGet("/", async ([AsParameters] GetDiscountCodesQuery query, ISender sender, CancellationToken cancellationToken) 
                => await sender.Send(query, cancellationToken))
            .WithName("GetDiscountCodes")
            .Produces<PaginatedResult<DiscountCodeDto>>()
            .WithDescription("Get all discount codes.")
            .WithOpenApi();

        group.MapGet("/{code}", async (string code, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new FindDiscountCodeQuery(code);
                var discountCode = await sender.Send(query, cancellationToken);
                return discountCode is null ? Results.NotFound() : Results.Ok(discountCode);
            })
            .WithName("FindDiscountCode")
            .Produces<DiscountCodeDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Find a discount code by code.")
            .WithOpenApi();

        group.MapPost("/", async (CreateDiscountCodeCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                await sender.Send(command, cancellationToken);
                return Results.Created($"/discount-codes/{command.Code}", command);
            })
            .WithName("CreateDiscountCode")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a new discount code.")
            .WithOpenApi();
        
        group.MapPatch("/{id:guid}", async (Guid id, UpdateDiscountCodeCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                command = command with { Id = id };
                await sender.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .WithName("UpdateDiscountCode")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update an existing discount code.")
            .WithOpenApi();
        
        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new DeleteDiscountCodeCommand(id);
                await sender.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .WithName("DeleteDiscountCode")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Delete a discount code.")
            .WithOpenApi();
    }
}