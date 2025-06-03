using TrackEasy.Domain.Shared;

namespace TrackEasy.Application.Services;

public sealed record TicketPriceResult(Money Price, List<int>? SeatNumbers);