using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Files.Abstractions;

namespace TrackEasy.Application.Tickets.GetQrCode;

public sealed record GetQrCodeQuery(Guid QrCodeId) : IQuery<FileModel?>;