using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Operators.GetCoaches;
using TrackEasy.Application.Operators.Shared;
using TrackEasy.Infrastructure.Database;
using static TrackEasy.Infrastructure.Queries.Operators.Extensions;
using TrackEasy.Shared.Pagination;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;




namespace TrackEasy.Infrastructure.Queries.Operators;

internal sealed class GetCoachesQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<GetCoachesQuery, PaginatedResult<CoachDto>>
{
    public async Task<PaginatedResult<CoachDto>> Handle(GetCoachesQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Coaches
            .AsNoTracking()
            .WithOperatorId(request.OperatorId)
            .WithCoachCode(request.Code)
            .Select(c => new CoachDto(c.Id, c.Code))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
