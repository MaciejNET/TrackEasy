using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Tickets.GetTickets;

public sealed record GetTicketsQuery(Guid UserId, TicketType Type, int PageNumber, int PageSize) : IQuery<PaginatedResult<TicketDto>>;

public enum TicketType
{
    CURRENT,
    ARCHIVED
}