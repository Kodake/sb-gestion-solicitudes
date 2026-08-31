namespace SB.GestionSolicitudes.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public IReadOnlyList<string> Errors { get; }

    protected Result(bool isSuccess, string message, IEnumerable<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Errors = (errors ?? Array.Empty<string>()).ToList().AsReadOnly();
    }

    public static Result Success(string message = "Operación realizada exitosamente")
        => new Result(true, message);

    public static Result Failure(string message, IEnumerable<string>? errors = null)
        => new Result(false, message, errors);

    public static Result Failure(string error)
        => new Result(false, error, new[] { error });
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string message, IEnumerable<string>? errors = null)
        : base(isSuccess, message, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value, string message = "Operación realizada exitosamente")
        => new Result<T>(true, value, message);

    public static new Result<T> Failure(string message, IEnumerable<string>? errors = null)
        => new Result<T>(false, default, message, errors);

    public static new Result<T> Failure(string error)
        => new Result<T>(false, default, error, new[] { error });
}
