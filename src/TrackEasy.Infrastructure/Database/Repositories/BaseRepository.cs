using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal abstract class BaseRepository(TrackEasyDbContext dbContext) : IBaseRepository
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}