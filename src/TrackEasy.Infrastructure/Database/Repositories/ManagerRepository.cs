using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Managers;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class ManagerRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IManagerRepository
{
    private readonly TrackEasyDbContext _dbContext = dbContext;
    public Task<Manager?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => _dbContext.Managers
            .Include(x => x.User)
            .Include(x => x.Operator)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
}