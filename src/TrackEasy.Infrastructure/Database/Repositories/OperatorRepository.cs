using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Operators;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class OperatorRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IOperatorRepository
{
    public Task<bool> ExistsAsync(string name, string code, CancellationToken cancellationToken)
        => dbContext.Operators.AnyAsync(x => x.Name == name && x.Code == code, cancellationToken);

    public Task<bool> ExistsAsync(Guid id, string name, string code, CancellationToken cancellationToken)
        => dbContext.Operators.AnyAsync(x => x.Id != id && x.Name == name && x.Code == code, cancellationToken);

    public async Task<Operator?> FindAsync(Guid id, CancellationToken cancellationToken)
        => await dbContext.Operators.FindAsync([id], cancellationToken);

    public void Add(Operator @operator)
    {
        dbContext.Operators.Add(@operator);
    }

    public void Delete(Operator @operator)
    {
        dbContext.Operators.Remove(@operator);
    }
}