namespace TrackEasy.Application.Services;

public interface IConnectionSeatsService
{
    Task<List<int>?> GetAvailableSeatsAsync(
        Guid connectionId,
        DateOnly date,
        Guid startStationId,
        Guid endStationId,
        CancellationToken cancellationToken);
}