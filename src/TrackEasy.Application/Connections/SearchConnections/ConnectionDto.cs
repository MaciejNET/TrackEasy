namespace TrackEasy.Application.Connections.SearchConnections;

public sealed record ConnectionDto(
    Guid Id,
    string Name,
    string OperatorName,
    string OperatorCode,
    TimeOnly DepartureTime,
    TimeOnly ArrivalTime,
    string DepartureStation,
    string ArrivalStation,
    decimal Price)
{
    public TimeOnly Duration => TimeOnly.FromTimeSpan(ArrivalTime - DepartureTime);
}