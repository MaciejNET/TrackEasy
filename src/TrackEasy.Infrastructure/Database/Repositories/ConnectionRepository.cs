using Microsoft.EntityFrameworkCore;
using TrackEasy.Domain.Connections;

namespace TrackEasy.Infrastructure.Database.Repositories;

internal sealed class ConnectionRepository(TrackEasyDbContext dbContext) : BaseRepository(dbContext), IConnectionRepository
{
    private readonly TrackEasyDbContext _dbContext = dbContext;
    
    public async Task<Connection?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _dbContext.Connections
            .Include(x => x.Operator)
            .Include(x => x.Stations)
                .ThenInclude(s => s.Station)
            .Include(x => x.Request)
            .Include(x => x.Schedule)
            .Include(x => x.Train)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Add(Connection connection)
    {
        _dbContext.Connections.Add(connection);
    }

    public void Delete(Connection connection)
    {
        _dbContext.Connections.Remove(connection);
    }
}