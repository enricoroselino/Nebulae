namespace Shared;

public class Result
{
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public int StatusCode { get; private set; }

    protected Result(bool isSuccess, int statusCode, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new Result(true, 200);

    public static Result Error() =>
        new Result(false, 500, "An unexpected error occurred while processing your request.");

    public static Result Conflict(string errorMessage) => new Result(false, 409, errorMessage);
    public static Result NotFound(string errorMessage) => new Result(false, 404, errorMessage);
    public static Result BadRequest(string errorMessage) => new Result(false, 400, errorMessage);
}

public sealed class Result<T> : Result where T : notnull
{
    private Result(T value, bool isSuccess, int statusCode, string? errorMessage = null)
        : base(isSuccess, statusCode, errorMessage)
    {
        Value = value;
    }

    public T Value { get; }

    public static Result Success(T value) => new Result<T>(value, isSuccess: true, 200, errorMessage: null);
}