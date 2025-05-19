using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.UpdateTrain;

public sealed record UpdateTrainCommand(Guid OperatorId, Guid TrainId, string Name, 
    IEnumerable<(Guid CoachId, int Number)> Coaches) : ICommand;