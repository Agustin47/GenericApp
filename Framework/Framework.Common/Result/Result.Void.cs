namespace Framework.Common.Result;

public class Result : Result<Result>
{
    private Result() : base() { }
    private Result(params ErrorValidation[] validationErrors) : base(validationErrors) { }
    private Result(string message, string errorCode) : base(message, errorCode) { }
    
    public static Result Success() => new();
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
    public new static Result Failed(params ErrorValidation[] validationErrors) => new(validationErrors);
    public new static Result Failed(string message, string errorCode) => new(message, errorCode);
}