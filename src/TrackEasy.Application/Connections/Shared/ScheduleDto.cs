namespace TrackEasy.Application.Connections.Shared;

public sealed record ScheduleDto(DateOnly ValidFrom, DateOnly ValidTo, List<DayOfWeek> DaysOfWeek);