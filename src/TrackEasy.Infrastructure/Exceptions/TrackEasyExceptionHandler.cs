using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Infrastructure.Exceptions;

internal sealed class TrackEasyExceptionHandler(ILogger<TrackEasyExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not TrackEasyException trackEasyException)
        {
            return false;
        }
        
        logger.LogError("Track easy exception occurred with code: {Code} and message: {Message}", trackEasyException.Code, trackEasyException.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = trackEasyException.Code,
            Title = trackEasyException.Message,
            Detail = trackEasyException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}