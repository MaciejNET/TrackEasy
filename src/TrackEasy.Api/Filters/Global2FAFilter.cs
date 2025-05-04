using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackEasy.Application.Security;

namespace TrackEasy.Api.Filters;

public sealed class Global2FAFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var endpoint = context.HttpContext.GetEndpoint();
        
        if (endpoint?.Metadata.GetMetadata<IAuthorizeData>() != null)
        {
            var userContext = context.HttpContext.RequestServices
                .GetRequiredService<IUserContext>();

            if (userContext is { IsAuthenticated: true, IsTwoFactorVerified: false or null })
            {
                return new ProblemDetails
                {
                    Title = "Two-factor authentication required",
                    Detail = "You must complete two-factor authentication to access this resource",
                    Status = StatusCodes.Status403Forbidden,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
                };
            }
        }

        return await next(context);
    }
}