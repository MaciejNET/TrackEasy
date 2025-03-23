using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.DiscountCodes.FindDiscountCode;
using TrackEasy.Application.DiscountCodes.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.DiscountCodes;

internal sealed class FindDiscountCodeQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<FindDiscountCodeQuery, DiscountCodeDto?>
{
    public async Task<DiscountCodeDto?> Handle(FindDiscountCodeQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.DiscountCodes
            .AsNoTracking()
            .Where(x => x.Code == request.Code)
            .Select(x => new DiscountCodeDto(x.Id, x.Code, x.Percentage, x.From, x.To))
            .SingleOrDefaultAsync(cancellationToken);
    }
}