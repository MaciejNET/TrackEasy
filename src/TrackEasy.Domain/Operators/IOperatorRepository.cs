using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Operators;

public interface IOperatorRepository : IBaseRepository
{
    Task<bool> ExistsAsync(string name, string code, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, string name, string code, CancellationToken cancellationToken);
    Task<Operator?> FindAsync(Guid id, CancellationToken cancellationToken);
    Task<Operator?> FindWithCoachesAsync(Guid id, CancellationToken cancellationToken);
    void Add(Operator @operator);
    void Delete(Operator @operator);
}