using MediatR;
using TrackEasy.Api.AuthorizationHandlers;
using TrackEasy.Application.Discounts.CreateDiscount;
using TrackEasy.Application.Discounts.DeleteDiscount;
using TrackEasy.Application.Discounts.FindDiscount;
using TrackEasy.Application.Discounts.GetDiscounts;
using TrackEasy.Application.Discounts.Shared;
using TrackEasy.Application.Discounts.UpdateDiscount;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public class DiscountsEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/discounts").WithTags("Discounts");

        group.MapGet("/", async ([AsParameters] GetDiscountsQuery query, ISender sender, CancellationToken cancellationToken) =>
            await sender.Send(query, cancellationToken))
            .RequireAdminAccess()
            .WithName("GetDiscounts")
            .Produces<PaginatedResult<DiscountDto>>()
            .WithDescription("Get all discounts.")
            .WithOpenApi();

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new FindDiscountQuery(id);
            var discount = await sender.Send(query, cancellationToken);
            return discount is null ? Results.NotFound() : Results.Ok(discount);
        })
            .RequireAdminAccess()
            .WithName("FindDiscount")
            .Produces<DiscountDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Find a discount by ID.")
            .WithOpenApi();

        group.MapPost("/", async (CreateDiscountCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(command, cancellationToken);
            return Results.Created("/discounts", command);
        })
            .RequireAdminAccess()
            .WithName("CreateDiscount")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a new discount.")
            .WithOpenApi();

        group.MapPatch("/{id:guid}", async (Guid id, UpdateDiscountCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            command = command with { Id = id };
            await sender.Send(command, cancellationToken);
            return Results.NoContent();
        })
            .RequireAdminAccess()
            .WithName("UpdateDiscount")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Update an existing discount.")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteDiscountCommand(id);
            await sender.Send(command, cancellationToken);
            return Results.NoContent();
        })
            .RequireAdminAccess()
            .WithName("DeleteDiscount")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Delete a discount.")
            .WithOpenApi();
    }
}
