namespace TrackEasy.Application.Connections.GetConnections;

public sealed record ConnectionDto(Guid Id, string Name, string StartStation, string EndStation,
    IEnumerable<DayOfWeek> DaysOfWeek, bool IsActive);