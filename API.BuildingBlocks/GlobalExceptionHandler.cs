using API.Shared.Constants;
using API.Shared.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.BuildingBlocks;

internal partial class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var timestamp = DateTimeOffset.UtcNow;

        var exceptionResult = exception switch
        {
            ValidationException validationException => ValidationExceptionHandler(
                httpContext,
                validationException,
                timestamp
            ),
            UnauthorizedException => UnauthorizedExceptionHandler(httpContext),
            DomainException domainException => DomainExceptionHandler(
                httpContext,
                domainException,
                timestamp
            ),
            _ => DefaultExceptionHandler(
                httpContext,
                exception,
                timestamp
            )
        };

        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = exceptionResult.code;
        await httpContext.Response.WriteAsJsonAsync(exceptionResult.problemDetails, cancellationToken);
        return true;
    }
}

internal partial class GlobalExceptionHandler
{
    private (ProblemDetails problemDetails, int code) DomainExceptionHandler(
        HttpContext httpContext,
        DomainException exception,
        DateTimeOffset timestamp)
    {
        _logger.LogError(
            "Domain exception at {Timestamp}. Path: {RequestPath}. Exception: {ExceptionMessage}",
            timestamp,
            httpContext.Request.Path,
            exception.Message);

        const int statusCode = StatusCodes.Status422UnprocessableEntity;
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = HttpTitle.DomainViolation,
            Detail = exception.Message,
            Instance = httpContext.Request.Path,
            Type = "https://example.com/probs/domain-error"
        };

        return (problemDetails, statusCode);
    }

    private (ProblemDetails problemDetails, int code) DefaultExceptionHandler(
        HttpContext httpContext,
        Exception exception,
        DateTimeOffset timestamp)
    {
        _logger.LogError(
            "Error occurred at {Timestamp}. Path: {RequestPath}. Exception: {ExceptionMessage}",
            timestamp,
            httpContext.Request.Path,
            exception.Message);

        const int statusCode = StatusCodes.Status500InternalServerError;
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = HttpTitle.InternalServerError,
            Detail = "An unexpected error occurred while processing your request.",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions.Add("errorId", Guid.NewGuid().ToString());
        return (problemDetails, statusCode);
    }

    private (ProblemDetails problemDetails, int code) ValidationExceptionHandler(
        HttpContext httpContext,
        ValidationException exception,
        DateTimeOffset timestamp)
    {
        _logger.LogError(
            "Validation error at {Timestamp}. Path: {RequestPath}. ValidationErrors: {ValidationErrors}.",
            timestamp, httpContext.Request.Path,
            exception.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

        const int statusCode = StatusCodes.Status400BadRequest;
        var problemDetails = new ProblemDetails()
        {
            Status = statusCode,
            Title = "Validation Error",
            Detail = "One or more validation errors occurred.",
            Instance = httpContext.Request.Path
        };

        var errors = exception.Errors.ToDictionary(c => c.PropertyName, c => c.ErrorMessage);
        problemDetails.Extensions.Add("errors", errors);
        return (problemDetails, statusCode);
    }

    private static (ProblemDetails problemDetails, int code) UnauthorizedExceptionHandler(HttpContext httpContext)
    {
        const int statusCode = StatusCodes.Status401Unauthorized;
        var problemDetails = new ProblemDetails()
        {
            Status = statusCode,
            Title = "Unauthorized",
            Detail = "This might be due to missing or invalid credentials. Please authenticate and try again.",
            Instance = httpContext.Request.Path
        };

        return (problemDetails, statusCode);
    }
}