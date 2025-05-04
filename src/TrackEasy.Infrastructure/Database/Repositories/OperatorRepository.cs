using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Operators;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class OperatorRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IOperatorRepository
{
    private readonly TrackEasyDbContext _dbContext = dbContext;
    public Task<bool> ExistsAsync(string name, string code, CancellationToken cancellationToken)
        => _dbContext.Operators.AnyAsync(x => x.Name == name && x.Code == code, cancellationToken);

    public Task<bool> ExistsAsync(Guid id, string name, string code, CancellationToken cancellationToken)
        => _dbContext.Operators.AnyAsync(x => x.Id != id && x.Name == name && x.Code == code, cancellationToken);

    public async Task<Operator?> FindAsync(Guid id, CancellationToken cancellationToken)
        => await _dbContext.Operators.FindAsync([id], cancellationToken);

    public Task<Operator?> FindWithCoachesAsync(Guid id, CancellationToken cancellationToken)
        => _dbContext.Operators
            .Include(x => x.Coaches)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Add(Operator @operator)
    {
        _dbContext.Operators.Add(@operator);
    }

    public void Delete(Operator @operator)
    {
        _dbContext.Operators.Remove(@operator);
    }
}