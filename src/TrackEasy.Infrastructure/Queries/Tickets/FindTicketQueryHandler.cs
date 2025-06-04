using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Tickets.FindTicket;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Tickets;

internal sealed class FindTicketQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<FindTicketQuery, TicketDetailsDto?>
{
    public async Task<TicketDetailsDto?> Handle(FindTicketQuery request, CancellationToken cancellationToken)
    {
        var ticket = await dbContext.Tickets
            .AsNoTracking()
            .Where(x => x.Id == request.TicketId)
            .Select(x => new TicketDetailsDto(
                x.Id,
                x.TicketNumber,
                x.Passengers.Select(p => new PersonDetailsDto(
                    p.FirstName,
                    p.LastName,
                    p.DateOfBirth,
                    p.Discount
                )),
                x.SeatNumbers,
                x.ConnectionDate,
                x.Stations.Select(s => new TicketConnectionStationDto(
                    s.Name,
                    s.ArrivalTime,
                    s.DepartureTime,
                    s.SequenceNumber
                )),
                x.OperatorCode,
                x.OperatorName,
                x.TrainName,
                x.QrCodeId,
                x.TicketStatus.ToString()
            ))
            .SingleOrDefaultAsync(cancellationToken);

        return ticket;
    }
}