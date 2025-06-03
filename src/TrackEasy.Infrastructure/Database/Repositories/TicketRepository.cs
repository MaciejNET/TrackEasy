using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Tickets;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class TicketRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), ITicketRepository
{
    private readonly TrackEasyDbContext _dbContext = dbContext;
    
    public Task<Ticket?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Tickets
            .Include(x => x.Stations)
            .Include(x => x.Passengers)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(Ticket ticket)
    {
        _dbContext.Tickets.Add(ticket);
    }
}