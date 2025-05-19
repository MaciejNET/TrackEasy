using TrackEasy.Application.Operators.GetCoaches;

namespace TrackEasy.Application.Operators.FindTrain;

public sealed record TrainDetailsDto(Guid Id, string Name, IEnumerable<(CoachDto Coach, int Number)> Coaches);