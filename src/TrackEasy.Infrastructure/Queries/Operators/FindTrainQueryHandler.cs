using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Operators.FindTrain;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Operators;

internal sealed class FindTrainQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<FindTrainQuery, TrainDetailsDto?>
{
    public async Task<TrainDetailsDto?> Handle(FindTrainQuery request, CancellationToken cancellationToken)
    {
        var trainEntity = await dbContext.Trains
            .AsNoTracking()
            .WithOperatorId(request.OperatorId)
            .WithTrainId(request.TrainId)
            .Select(x => new
            {
                x.Id,
                x.Name,
                Coaches = x.Coaches.Select(c => new
                {
                    CoachId   = c.Coach.Id,
                    CoachCode = c.Coach.Code,
                    Number    = c.Number
                })
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (trainEntity is null)
            return null;

        var coaches = trainEntity.Coaches
            .Select(c => (new CoachDto(c.CoachId, c.CoachCode), c.Number));

        return new TrainDetailsDto(trainEntity.Id, trainEntity.Name, coaches);
    }
}