using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Discounts.FindDiscount;
using TrackEasy.Application.Discounts.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Discounts;

internal sealed class FindDiscountQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<FindDiscountQuery, DiscountDto?>
{
    public async Task<DiscountDto?> Handle(FindDiscountQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Discounts
            .AsNoTracking()
            .WithDiscountId(request.Id)
            .Select(x => new DiscountDto(x.Id, x.Name, x.Percentage))
            .SingleOrDefaultAsync(cancellationToken);
    }
}