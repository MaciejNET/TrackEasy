using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Operators.GetTrains;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Operators;

internal sealed class GetTrainsQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetTrainsQuery, PaginatedResult<TrainDto>>
{
    public async Task<PaginatedResult<TrainDto>> Handle(GetTrainsQuery request, CancellationToken cancellationToken)
    {
        var trains = await dbContext.Trains
            .AsNoTracking()
            .WithOperatorId(request.OperatorId)
            .WithTrainName(request.Name)
            .OrderBy(x => x.Name)
            .Select(x => new TrainDto(x.Id, x.Name))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
        
        return trains;
    }
}