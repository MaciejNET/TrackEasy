using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.RefundRequests.FindRefundRequest;
using TrackEasy.Application.Tickets.FindTicket;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.RefundRequests;

internal sealed class FindRefundRequestQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<FindRefundRequestQuery, RefundRequestDetailsDto?>
{
    public async Task<RefundRequestDetailsDto?> Handle(FindRefundRequestQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.RefundRequests
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new RefundRequestDetailsDto(
                x.Id,
                x.Ticket.TicketNumber,
                x.Ticket.Passengers.Select(p => new PersonDetailsDto(
                    p.FirstName,
                    p.LastName,
                    p.DateOfBirth,
                    p.Discount
                )),
                x.Ticket.SeatNumbers,
                x.Ticket.ConnectionDate,
                x.Ticket.Stations.Select(s => new TicketConnectionStationDto(
                    s.Name,
                    s.ArrivalTime,
                    s.DepartureTime,
                    s.SequenceNumber
                )),
                x.Ticket.OperatorCode,
                x.Ticket.OperatorName,
                x.Ticket.TrainName,
                x.Ticket.QrCodeId,
                x.Ticket.TicketStatus.ToString(),
                x.Reason,
                x.CreatedAt
            ))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
