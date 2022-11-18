namespace WindowThing;

public class Result
{
    public static Result<T> Success<T>(T value)
    {
        return Result<T>.Success(value);
    }
    public static Result<T> Fail<T>(string? message)
    {
        return Result<T>.Fail(message);
    }
}

public class Result<T>
{
    public bool IsSuccess { get; }

    public T? Value { get; }

    public string? Error { get; }

    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }

    private Result(string? message)
    {
        Error = message;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Fail(string? message)
    {
        return new Result<T>(message);
    }
}
