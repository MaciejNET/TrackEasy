using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.RefundRequests.GetRefundRequests;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.RefundRequests;

internal sealed class GetRefundRequestsQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<GetRefundRequestsQuery, PaginatedResult<RefundRequestDto>>
{
    public async Task<PaginatedResult<RefundRequestDto>> Handle(GetRefundRequestsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.RefundRequests
            .AsNoTracking()
            .Where(x => x.Ticket.OperatorId == request.OperatorId)
            .Where(x => x.Ticket.TicketStatus == Domain.Tickets.TicketStatus.PAID)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new RefundRequestDto(
                x.Id,
                x.TicketId,
                x.Ticket.TicketNumber,
                x.Ticket.EmailAddress,
                x.Ticket.ConnectionDate,
                x.Reason,
                x.CreatedAt
            ))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
