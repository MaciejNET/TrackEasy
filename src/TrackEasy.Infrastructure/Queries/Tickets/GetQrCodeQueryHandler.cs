using Microsoft.Extensions.Configuration;
using TrackEasy.Application.Tickets.GetQrCode;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Files.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Tickets;

internal sealed class GetQrCodeQueryHandler(IBlobService blobService, IConfiguration configuration) : IQueryHandler<GetQrCodeQuery, FileModel?>
{
    private readonly string _qrCodeContainerName = configuration["qr-code-container"] ?? throw new ArgumentNullException(nameof(configuration));
    
    public async Task<FileModel?> Handle(GetQrCodeQuery request, CancellationToken cancellationToken)
    {
        var blob = await blobService.FindAsync(request.QrCodeId.ToString(), _qrCodeContainerName, cancellationToken);

        return blob;
    }
}