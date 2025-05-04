using TrackEasy.Application.Operators.GetOperators;
using TrackEasy.Application.Operators.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Operators;

internal sealed class GetOperatorsQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetOperatorsQuery, PaginatedResult<OperatorDto>>
{
    public async Task<PaginatedResult<OperatorDto>> Handle(GetOperatorsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Operators
            .WithName(request.Name)
            .WithCode(request.Code)
            .Select(x => new OperatorDto(x.Id, x.Name, x.Code))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}