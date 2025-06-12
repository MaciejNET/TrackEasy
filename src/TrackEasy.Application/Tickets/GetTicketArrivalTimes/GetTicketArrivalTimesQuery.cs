using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.GetTicketArrivalTimes;

public sealed record GetTicketArrivalTimesQuery(Guid Id) : IQuery<IEnumerable<TicketArrivalDto>>;
