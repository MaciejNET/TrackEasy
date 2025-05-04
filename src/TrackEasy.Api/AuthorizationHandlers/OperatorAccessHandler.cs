using Microsoft.AspNetCore.Authorization;
using TrackEasy.Application.Security;

namespace TrackEasy.Api.AuthorizationHandlers;

public sealed class OperatorAccessRequirement(Guid operatorId) : IAuthorizationRequirement
{
    public Guid OperatorId { get; } = operatorId;
}

public sealed class OperatorAccessHandler : AuthorizationHandler<OperatorAccessRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperatorAccessRequirement requirement)
    {
        var operatorIdClaim = context.User.FindFirst(CustomClaims.OperatorId);
        if (operatorIdClaim is not null &&
            Guid.TryParse(operatorIdClaim.Value, out var operatorId) &&
            operatorId == requirement.OperatorId)
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}