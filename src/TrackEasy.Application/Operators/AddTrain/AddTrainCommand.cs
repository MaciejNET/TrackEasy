using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.AddTrain;

public sealed record AddTrainCommand(Guid OperatorId, string Name, IEnumerable<(Guid CoachId, int Number)> Coaches) : ICommand<Guid>;