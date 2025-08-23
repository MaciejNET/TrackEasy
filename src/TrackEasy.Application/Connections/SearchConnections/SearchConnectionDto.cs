using TrackEasy.Application.Shared;

namespace TrackEasy.Application.Connections.SearchConnections;

public sealed record SearchConnectionDto(
    Guid Id,
    string Name,
    string OperatorName,
    string OperatorCode,
    TimeOnly DepartureTime,
    TimeOnly ArrivalTime,
    Guid DepartureStationId,
    string DepartureStation,
    Guid ArrivalStationId,
    string ArrivalStation,
    MoneyDto Price)
{
    public TimeOnly Duration => TimeOnly.FromTimeSpan(ArrivalTime - DepartureTime);
}