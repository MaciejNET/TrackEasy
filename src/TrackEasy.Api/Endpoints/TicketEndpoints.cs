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

namespace TrackEasy.Api.Endpoints;

public class TicketEndpoints : IEndpoints
{
    public static void MapEndpoints(RouteGroupBuilder rootGroup)
    {
        var group = rootGroup.MapGroup("tickets").WithTags("Tickets");
        
        group.MapPost("/", async (BuyTicketCommand command, ISender sender, CancellationToken ct) => 
            Results.Ok(await sender.Send(command, ct)))
            .WithName("BuyTicket")
            .WithDescription("Purchase tickets for specified connections");
        
        group.MapGet("/{userId:guid}", async (Guid userId, int pageNumber, int pageSize, TicketType type, ISender sender, CancellationToken ct) => 
            Results.Ok(await sender.Send(new GetTicketsQuery(userId, type, pageNumber, pageSize), ct)))
            .WithName("GetTickets")
            .WithDescription("Get paginated tickets for a user");
        
        group.MapGet("/{ticketId:guid}/details", async (Guid ticketId, ISender sender, CancellationToken ct) => 
        {
            var result = await sender.Send(new FindTicketQuery(ticketId), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
            .WithName("GetTicketDetails")
            .WithDescription("Get details of a specific ticket");
        
        group.MapGet("/{id:guid}/cities", async (Guid id, ISender sender, CancellationToken ct) => 
            Results.Ok(await sender.Send(new GetTicketCitiesQuery(id), ct)))
            .WithName("GetTicketCities")
            .WithDescription("Get cities related to a ticket");
        
        group.MapGet("/qr-code/{qrCodeId:guid}", async (Guid qrCodeId, ISender sender, CancellationToken ct) =>
        {
            var qrCode = await sender.Send(new GetQrCodeQuery(qrCodeId), ct);
            return qrCode is not null ? Results.File(qrCode.Content, qrCode.ContentType) : Results.NotFound();
        })
        .WithName("GetTicketQrCode")
        .WithDescription("Get the QR code for a ticket by its ID");
        
        group.MapPost("/payment/card", async (PayTicketByCardCommand command, ISender sender, CancellationToken ct) => 
        {
            await sender.Send(command, ct);
            return Results.NoContent();
        })
            .WithName("PayTicketsByCard")
            .WithDescription("Pay for tickets using card");
        
        group.MapPost("/payment/cash/{ticketId:guid}", async (Guid ticketId, ISender sender, CancellationToken ct) => 
        {
            await sender.Send(new PayTicketByCashCommand(ticketId), ct);
            return Results.NoContent();
        })
            .WithName("PayTicketByCash")
            .WithDescription("Pay for a ticket with cash");
        
        group.MapGet("/current", async (ISender sender, CancellationToken ct) =>
        {
            var ticketId = await sender.Send(new FindCurrentTicketIdQuery(), ct);
            return ticketId is not null ? Results.Ok(ticketId) : Results.NotFound();
        })
            .WithName("GetCurrentTicketId")
            .WithDescription("Get the ID of the current ticket for the user");
        
        group.MapPost("/refund-request",
            async (CreateRefundRequestCommand command, ISender sender, CancellationToken ct) =>
            {
                await sender.Send(command, ct);
                return Results.NoContent();
            })
            .WithName("CreateRefundRequest")
            .WithDescription("Create a refund request for a ticket");
            
        group.MapPost("/{ticketId:guid}/cancel",
            async (Guid ticketId, ISender sender, CancellationToken ct) =>
            {
                await sender.Send(new CancelTicketCommand(ticketId), ct);
                return Results.NoContent();
            })
            .WithName("CancelTicket")
            .WithDescription("Cancel a ticket by its ID");
    }
}