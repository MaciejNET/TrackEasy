using TrackEasy.Application.Operators.FindTrain;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Operators;

internal sealed class FindTrainQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<FindTrainQuery, TrainDetailsDto?>
{
    public Task<TrainDetailsDto?> Handle(FindTrainQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}