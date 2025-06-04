using TrackEasy.Application.Connections.Shared;
using TrackEasy.Application.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Connections.CreateConnection;

public sealed record CreateConnectionCommand(string Name, Guid OperatorId, MoneyDto PricePerKilometer, Guid TrainId,
    ScheduleDto Schedule, List<ConnectionStationDto> ConnectionStations, bool NeedsSeatReservation) : ICommand<Guid>;