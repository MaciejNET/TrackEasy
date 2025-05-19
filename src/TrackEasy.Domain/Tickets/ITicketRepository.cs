using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Tickets;

public interface ITicketRepository : IBaseRepository
{
    Task<Ticket?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
}