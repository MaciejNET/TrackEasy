using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Operators.FindOperator;
using TrackEasy.Application.Operators.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Operators;

internal sealed class FindOperatorQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<FindOperatorQuery, OperatorDto?>
{
    public async Task<OperatorDto?> Handle(FindOperatorQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Operators
            .AsNoTracking()
            .WithOperatorId(request.Id)
            .Select(x => new OperatorDto(x.Id, x.Name, x.Code))
            .SingleOrDefaultAsync(cancellationToken);
    }
}