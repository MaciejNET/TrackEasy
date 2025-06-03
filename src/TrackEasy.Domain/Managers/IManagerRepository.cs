using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Managers;

public interface IManagerRepository : IBaseRepository
{
    Task<Manager?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<Manager>> GetByOperatorIdAsync(Guid operatorId, CancellationToken cancellationToken);
}