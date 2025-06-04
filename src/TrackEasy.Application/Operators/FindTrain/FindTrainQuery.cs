using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.FindTrain;

public sealed record FindTrainQuery(Guid OperatorId, Guid TrainId) : IQuery<TrainDetailsDto?>;