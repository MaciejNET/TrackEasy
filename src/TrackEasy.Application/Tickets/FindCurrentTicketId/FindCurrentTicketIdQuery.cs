using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.FindCurrentTicketId;

public sealed record FindCurrentTicketIdQuery : IQuery<Guid?>;