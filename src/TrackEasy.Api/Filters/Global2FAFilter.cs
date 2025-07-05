using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackEasy.Application.Security;

namespace TrackEasy.Api.Filters;

public sealed class Global2FAFilter : IEndpointFilter
{
    private static readonly string[] ExcludedPaths =
    [
        "/users/token-2fa",
        "/swagger",
        "/swagger/index.html",
        "/health",
        "/users/external"
    ];

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var path = context.HttpContext.Request.Path.Value;
        if (ExcludedPaths.Any(p => path!.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
            return await next(context);

        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAuthorizeData>() != null)
        {
            var userContext = context.HttpContext.RequestServices
                .GetRequiredService<IUserContext>();

            if (userContext.IsAuthenticated && userContext.IsTwoFactorVerified != true)
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