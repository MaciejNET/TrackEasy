using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.BuyTicket;

public sealed record BuyTicketCommand(
    string Email,
    IEnumerable<PersonDto> Passengers,
    Guid? DiscountCodeId,
    IEnumerable<TicketConnectionDto> Connections
    ) : ICommand<IReadOnlyCollection<Guid>>;
    
public sealed record TicketConnectionDto(
    Guid Id,
    Guid StartStationId,
    Guid EndStationId,
    DateOnly ConnectionDate);