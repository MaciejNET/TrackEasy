using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.GetTicketCities;

public sealed record GetTicketCitiesQuery(Guid Id) : IQuery<IEnumerable<TicketCityDto>>;