using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Tickets.GetTickets;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Tickets;

internal sealed class GetTicketsQueryHandler(TrackEasyDbContext dbContext, TimeProvider timeProvider) : IQueryHandler<GetTicketsQuery, PaginatedResult<TicketDto>>
{
    public async Task<PaginatedResult<TicketDto>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await dbContext.Tickets
            .AsNoTracking()
            .WithTicketType(request.Type, timeProvider)
            .WithUserId(request.UserId)
            .Select(x => new TicketDto(
                x.Id,
                x.Stations.Single(s => s.SequenceNumber == 1).Name,
                x.Stations.Single(s => s.SequenceNumber == x.Stations.Count).Name,
                x.Stations.Single(s => s.SequenceNumber == 1).DepartureTime.GetValueOrDefault(),
                x.Stations.Single(s => s.SequenceNumber == x.Stations.Count).ArrivalTime.GetValueOrDefault(),
                x.ConnectionDate
            ))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);

        return tickets;
    }
}