using Microsoft.Extensions.Configuration;
using QRCoder;
using TrackEasy.Domain.Tickets;
using TrackEasy.Mails.Abstractions;
using TrackEasy.Mails.Abstractions.Models;
using TrackEasy.Pdf.Abstractions;
using TrackEasy.Pdf.Abstractions.Models;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Files.Abstractions;

namespace TrackEasy.Application.Tickets.TicketPayed;

internal sealed class TicketPayedEventHandler(
    IPdfService pdfService,
    IEmailSender emailSender,
    IBlobService blobService,
    IConfiguration configuration
    ) : IDomainEventHandler<TicketPayedEvent>
{
    private readonly string _pdfContainerName = configuration["pdf-container"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string _qrCodeContainerName = configuration["qr-code-container"] ?? throw new ArgumentNullException(nameof(configuration));
    
    public async Task Handle(TicketPayedEvent notification, CancellationToken cancellationToken)
    {
        var ticket = notification.Ticket;
        var qrCode = GenerateQrCode(ticket.Id);

        var startStation = ticket.Stations.First(x => x.SequenceNumber == 1);
        var endStation   = ticket.Stations.First(x => x.SequenceNumber == ticket.Stations.Count);

        var model = new TicketModel(
            ticket.Id,
            ticket.TicketNumber,
            ticket.ConnectionName,
            ticket.TrainName,
            ticket.OperatorName,
            ticket.ConnectionDate,
            startStation.Name,
            startStation.DepartureTime.GetValueOrDefault(),
            endStation.Name,
            endStation.ArrivalTime.GetValueOrDefault(),
            ticket.Passengers.Select(p => new PassengerModel(p.FirstName, p.LastName)),
            ticket.SeatNumbers,
            ticket.Price.Amount,
            ticket.Price.Currency.ToString(),
            ticket.TicketStatus.ToString(),
            qrCode
        );
        
        var pdf = await pdfService.GenerateTicketPdfAsync(model, cancellationToken);
        
        var qrCodeId = Guid.NewGuid();
        await blobService.SaveAsync(qrCodeId.ToString(), qrCode, "image/png", _qrCodeContainerName, cancellationToken);
        ticket.SetQrCodeId(qrCodeId);
        
        await blobService.SaveAsync(ticket.Id.ToString(), pdf, "application/pdf", _pdfContainerName, cancellationToken);
        
        await emailSender.SendTicketEmailAsync(ticket.EmailAddress, new TicketEmailModel(ticket.TicketNumber, pdf));
    }

    private static byte[] GenerateQrCode(Guid ticketId)
    {
        using var qrCodeGenerator = new QRCodeGenerator();
        using var qrCodeData = qrCodeGenerator.CreateQrCode(ticketId.ToString(), QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        
        return qrCode.GetGraphic(20);
    }
}