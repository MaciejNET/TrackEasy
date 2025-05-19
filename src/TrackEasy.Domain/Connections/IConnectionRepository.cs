using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Connections;

public interface IConnectionRepository : IBaseRepository
{
    Task<Connection?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
}