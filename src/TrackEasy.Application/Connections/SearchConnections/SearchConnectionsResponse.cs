namespace TrackEasy.Application.Connections.SearchConnections;

public sealed record SearchConnectionsResponse(
    List<ConnectionDto> Connections,
    int TransfersCount,
    string StartStation,
    string EndStation,
    TimeOnly DepartureTime,
    TimeOnly ArrivalTime)
{
    public TimeOnly TotalDuration => TimeOnly.FromTimeSpan(ArrivalTime - DepartureTime);
}