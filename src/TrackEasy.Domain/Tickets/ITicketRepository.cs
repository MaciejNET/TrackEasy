using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Tickets;

public interface ITicketRepository : IBaseRepository
{
    Task<Ticket?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Ticket?> FindByNumberAsync(int number, CancellationToken cancellationToken);
    void Add(Ticket ticket);
}