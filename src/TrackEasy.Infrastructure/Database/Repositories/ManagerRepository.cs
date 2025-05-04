using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Managers;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class ManagerRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IManagerRepository
{
    public Task<Manager?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => dbContext.Managers
            .Include(x => x.User)
            .Include(x => x.Operator)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
}