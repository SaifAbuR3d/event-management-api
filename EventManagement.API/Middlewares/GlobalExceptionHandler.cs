using EventManagement.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace EventManagement.API.Middlewares;

/// <summary>
/// Middleware for exception handling, it will catch all exceptions and return a proper response
public class GlobalExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Handles the exception
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>true if the exception was properly handled</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = MapException(exception);

        await Results.Problem(
            title: title,
            statusCode: statusCode,
            detail: detail
            ).ExecuteAsync(httpContext);

        return true;
    }



    private static (int statusCode, string title, string details) MapException(Exception exception)
    {
        return exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
            BadFileException => (StatusCodes.Status400BadRequest, "Bad File", exception.Message),
            BadRequestException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            UnauthenticatedException => (StatusCodes.Status401Unauthorized, "Unauthenticated", exception.Message),
            NoRolesException => (StatusCodes.Status403Forbidden, "Unauthorized", exception.Message),
            UnauthorizedException => (StatusCodes.Status403Forbidden, "Unauthorized", exception.Message),


            // FluentValidation exception, thrown explicitly with validator.ValidateAndThrowAsync when needed
            // (e.g. when not using FluentValidationAutoValidation)
            ValidationException validationException => (StatusCodes.Status400BadRequest, "Validation Error", validationException.Errors.First().ToString()),


            _ => (StatusCodes.Status500InternalServerError, "Something went wrong", "We made a mistake but we are working on it")
        };
    }
}
