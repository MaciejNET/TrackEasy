using TrackEasy.Application.Operators.GetCoaches;

namespace TrackEasy.Application.Operators.FindTrain;

public sealed record TrainDetailsDto(Guid Id, string Name, IEnumerable<TrainCoachDto> Coaches);

public sealed record TrainCoachDto(CoachDto Coach, int Number);