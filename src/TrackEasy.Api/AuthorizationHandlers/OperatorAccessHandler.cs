using Microsoft.AspNetCore.Authorization;
using TrackEasy.Application.Security;

namespace TrackEasy.Api.AuthorizationHandlers;

public sealed class OperatorAccessRequirement : IAuthorizationRequirement;

public sealed class OperatorAccessHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<OperatorAccessRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperatorAccessRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return Task.CompletedTask;

        var routeId = httpContext.GetRouteValue("id")?.ToString();
        if (!Guid.TryParse(routeId, out var operatorId))
        {
            return Task.CompletedTask;
        }

        var userOperatorId = context.User.FindFirst(CustomClaims.OperatorId)?.Value;
        if (!Guid.TryParse(userOperatorId, out var userOperatorGuid))
        {
            return Task.CompletedTask;
        }

        if (userOperatorGuid == operatorId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}