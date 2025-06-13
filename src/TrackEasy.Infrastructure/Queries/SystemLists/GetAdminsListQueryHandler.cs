using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.SystemLists;
using TrackEasy.Domain.Users;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.SystemLists;

internal sealed class GetAdminsListQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetAdminsListQuery, IEnumerable<SystemListItemDto>>
{
    public async Task<IEnumerable<SystemListItemDto>> Handle(GetAdminsListQuery request, CancellationToken cancellationToken)
    {
        var adminRoleId = await dbContext.Roles
            .Where(role => role.Name == Roles.Admin)
            .Select(role => role.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return await (
                from userRole in dbContext.UserRoles
                where userRole.RoleId == adminRoleId
                join user in dbContext.Users on userRole.UserId equals user.Id
                orderby user.FirstName, user.LastName
                select new SystemListItemDto(user.Id, $"{user.FirstName} {user.LastName}")
            ).ToListAsync(cancellationToken);
    }
}
