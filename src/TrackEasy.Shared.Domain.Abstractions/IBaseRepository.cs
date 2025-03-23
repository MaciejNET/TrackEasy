namespace TrackEasy.Shared.Domain.Abstractions;

public interface IBaseRepository
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}