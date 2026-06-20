using Microsoft.AspNetCore.Diagnostics;

namespace EnergyMix.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            InvalidHoursException => StatusCodes.Status400BadRequest, _ => StatusCodes.Status500InternalServerError
        };

        var message = exception switch
        {
            InvalidHoursException => exception.Message, _ => "Internal server error"
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            success = false,
            error = message
        }, cancellationToken);

        return true;
    }
}