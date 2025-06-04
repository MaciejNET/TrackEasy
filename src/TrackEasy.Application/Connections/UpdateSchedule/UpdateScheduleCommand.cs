using TrackEasy.Application.Connections.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.UpdateSchedule;

public sealed record UpdateScheduleCommand(Guid Id, ScheduleDto Schedule, List<ConnectionStationDto> ConnectionStations) : ICommand;