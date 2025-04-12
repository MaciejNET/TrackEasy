using MediatR;
using TrackEasy.Application.Discounts.CreateDiscount;
using TrackEasy.Application.Discounts.DeleteDiscount;
using TrackEasy.Application.Discounts.FindDiscount;
using TrackEasy.Application.Discounts.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TrackEasy.Api.Endpoints;

public static class DiscountsEndpoints
{
    public static void MapDiscountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/discounts").WithTags("Discounts");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new FindDiscountQuery(id);
            var discount = await sender.Send(query, cancellationToken);
            return discount is null ? Results.NotFound() : Results.Ok(discount);
        })
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
            .WithName("CreateDiscount")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a new discount.")
            .WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new DeleteDiscountCommand(id);
            await sender.Send(command, cancellationToken);
            return Results.NoContent();
        })
            .WithName("DeleteDiscount")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Delete a discount.")
            .WithOpenApi();
    }
}
