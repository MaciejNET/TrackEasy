using TrackEasy.Application.Operators.GetTrains;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Operators;

internal sealed class GetTrainsQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetTrainsQuery, PaginatedResult<TrainDto>>
{
    public Task<PaginatedResult<TrainDto>> Handle(GetTrainsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}