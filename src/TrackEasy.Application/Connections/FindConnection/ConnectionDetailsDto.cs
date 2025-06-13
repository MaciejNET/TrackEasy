using TrackEasy.Application.Shared;

namespace TrackEasy.Application.Connections.FindConnection;

public sealed record ConnectionDetailsDto(
    Guid Id,
    string Name,
    MoneyDto PricePerKilometer,
    Guid TrainId,
    string TrainName,
    DateOnly ValidFrom,
    DateOnly ValidTo,
    IEnumerable<DayOfWeek> DaysOfWeek,
    IEnumerable<ConnectionStationDetailsDto> Stations,
    bool HasPendingRequest,
    bool NeedsSeatReservation,
    bool IsActive);