using MediatR;
using TrackEasy.Application.RefundRequests.CreateRefundRequest;
using TrackEasy.Application.Tickets.BuyTicket;
using TrackEasy.Application.Tickets.CancelTicket;
using TrackEasy.Application.Tickets.FindCurrentTicketId;
using TrackEasy.Application.Tickets.FindTicket;
using TrackEasy.Application.Tickets.GetQrCode;
using TrackEasy.Application.Tickets.GetTicketCities;
using TrackEasy.Application.Tickets.GetTickets;
using TrackEasy.Application.Tickets.PayTicketByCard;
using TrackEasy.Application.Tickets.PayTicketByCash;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Api.Endpoints;

public class TicketEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("tickets").WithTags("Tickets");
        
        group.MapPost("/", async (BuyTicketCommand command, ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(command, ct)))
            .WithName("BuyTicket")
            .Produces<IReadOnlyCollection<Guid>>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Purchase tickets for specified connections")
            .WithOpenApi();
        
        group.MapGet("/{userId:guid}", async (Guid userId, int pageNumber, int pageSize, TicketType type, ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(new GetTicketsQuery(userId, type, pageNumber, pageSize), ct)))
            .WithName("GetTickets")
            .Produces<PaginatedResult<TicketDto>>()
            .WithDescription("Get paginated tickets for a user")
            .WithOpenApi();
        
        group.MapGet("/{ticketId:guid}/details", async (Guid ticketId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new FindTicketQuery(ticketId), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
            .WithName("GetTicketDetails")
            .Produces<TicketDetailsDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Get details of a specific ticket")
            .WithOpenApi();
        
        group.MapGet("/{id:guid}/cities", async (Guid id, ISender sender, CancellationToken ct) =>
            Results.Ok(await sender.Send(new GetTicketCitiesQuery(id), ct)))
            .WithName("GetTicketCities")
            .Produces<IEnumerable<TicketCityDto>>()
            .WithDescription("Get cities related to a ticket")
            .WithOpenApi();
        
        group.MapGet("/qr-code/{qrCodeId:guid}", async (Guid qrCodeId, ISender sender, CancellationToken ct) =>
        {
            var qrCode = await sender.Send(new GetQrCodeQuery(qrCodeId), ct);
            return qrCode is not null ? Results.File(qrCode.Content, qrCode.ContentType) : Results.NotFound();
        })
        .WithName("GetTicketQrCode")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithDescription("Get the QR code for a ticket by its ID")
        .WithOpenApi();
        
        group.MapPost("/payment/card", async (PayTicketByCardCommand command, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(command, ct);
            return Results.NoContent();
        })
            .WithName("PayTicketsByCard")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Pay for tickets using card")
            .WithOpenApi();
        
        group.MapPost("/payment/cash/{ticketId:guid}", async (Guid ticketId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new PayTicketByCashCommand(ticketId), ct);
            return Results.NoContent();
        })
            .WithName("PayTicketByCash")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Pay for a ticket with cash")
            .WithOpenApi();
        
        group.MapGet("/current", async (ISender sender, CancellationToken ct) =>
        {
            var ticketId = await sender.Send(new FindCurrentTicketIdQuery(), ct);
            return ticketId is not null ? Results.Ok(ticketId) : Results.NotFound();
        })
            .WithName("GetCurrentTicketId")
            .Produces<Guid>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Get the ID of the current ticket for the user")
            .WithOpenApi();
        
        group.MapPost("/refund-request",
            async (CreateRefundRequestCommand command, ISender sender, CancellationToken ct) =>
            {
                await sender.Send(command, ct);
                return Results.NoContent();
            })
            .WithName("CreateRefundRequest")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Create a refund request for a ticket")
            .WithOpenApi();
            
        group.MapPost("/{ticketId:guid}/cancel",
            async (Guid ticketId, ISender sender, CancellationToken ct) =>
            {
                await sender.Send(new CancelTicketCommand(ticketId), ct);
                return Results.NoContent();
            })
            .WithName("CancelTicket")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithDescription("Cancel a ticket by its ID")
            .WithOpenApi();
    }
}