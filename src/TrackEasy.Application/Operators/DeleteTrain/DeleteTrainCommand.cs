using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.DeleteTrain;

public sealed record DeleteTrainCommand(Guid OperatorId, Guid TrainId) : ICommand;