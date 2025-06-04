using TrackEasy.Pdf.Abstractions.Models;

namespace TrackEasy.Pdf.Abstractions;

public interface IPdfService
{
    Task<byte[]> GeneratePdfAsync(string htmlContent, CancellationToken cancellationToken);
    Task<byte[]> GenerateTicketPdfAsync(TicketModel model, CancellationToken cancellationToken);
}