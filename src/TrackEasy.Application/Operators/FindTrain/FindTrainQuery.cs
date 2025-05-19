using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.FindTrain;

public sealed record FindTrainQuery(Guid TrainId) : IQuery<TrainDetailsDto?>;