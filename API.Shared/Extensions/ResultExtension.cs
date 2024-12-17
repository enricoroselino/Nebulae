using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace API.Shared.Extensions;

public static class ResultExtension
{
    public static IResult ToMinimalApiResult(this Result result)
    {
        if (result.IsSuccess) return Results.Ok();

        var problemDetails = ProblemDetailsFactory.Create(result);
        return Results.Problem(problemDetails);
    }

    public static IResult ToMinimalApiResult<T>(this Result<T> result) where T : notnull
    {
        if (result.IsSuccess) return Results.Ok(result.Value);

        var problemDetails = ProblemDetailsFactory.Create(result);
        return Results.Problem(problemDetails);
    }
}

internal static class ProblemDetailsFactory
{
    public static ProblemDetails Create(Result result)
    {
        var title = result.StatusCode switch
        {
            400 => "Bad Request",
            409 => "Conflict",
            404 => "Not Found",
            _ => "Internal Server Error"
        };

        var problemDetails = new ProblemDetails
        {
            Status = result.StatusCode,
            Title = title,
            Detail = result.ErrorMessage
        };

        return problemDetails;
    }
}