using MediatR;
using TrackEasy.Application.SystemLists;

namespace TrackEasy.Api.Endpoints;

public sealed class SystemListsEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("/system-lists").WithTags("System Lists");
        
        group.MapGet("/cities", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetCitiesListQuery(), cancellationToken))
            .RequireAuthorization()
            .WithName("GetCitiesList")
            .Produces<IEnumerable<SystemListItemDto>>()
            .WithDescription("Get list of cities.")
            .WithOpenApi();

        group.MapGet("/stations", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetStationListQuery(), cancellationToken))
            .WithName("GetStationsList")
            .Produces<IEnumerable<SystemListItemDto>>()
            .WithDescription("Get list of stations.")
            .WithOpenApi();

        group.MapGet("/operators", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetOperatorsListQuery(), cancellationToken))
            .RequireAuthorization()
            .WithName("GetOperatorsList")
            .Produces<IEnumerable<SystemListItemDto>>()
            .WithDescription("Get list of operators.")
            .WithOpenApi();

        group.MapGet("/discounts", async (ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetDiscountsListQuery(), cancellationToken))
            .WithName("GetDiscountsList")
            .Produces<IEnumerable<SystemListItemDto>>()
            .WithDescription("Get list of discounts.")
            .WithOpenApi();

        group.MapGet("/managers/{operatorId:guid}", async (Guid operatorId, ISender sender, CancellationToken cancellationToken) =>
                await sender.Send(new GetManagersListQuery(operatorId), cancellationToken))
            .RequireAuthorization()
            .WithName("GetManagersList")
            .Produces<IEnumerable<SystemListItemDto>>()
            .WithDescription("Get list of managers for an operator.")
            .WithOpenApi();
    }
}