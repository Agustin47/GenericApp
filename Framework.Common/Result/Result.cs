namespace Framework.Common.Result;

public class Result<T>
{
    private List<ValidationError> _validationErrors;
    public T? Value { get; private set; }

    private Result(T? value)
    {
        Value = value;
        _validationErrors = new List<ValidationError>();
    }

    protected Result(params ValidationError[] validationErrors)
    {
        Value = default;
        _validationErrors = new List<ValidationError>(validationErrors);
    }

    protected Result(string error, string errorCode)
    {
        Value = default;
        _validationErrors = new List<ValidationError>() { new ValidationError(error, errorCode) };
    }

    public IReadOnlyCollection<ValidationError> ValidationErrors => _validationErrors.AsReadOnly();

    public bool IsSuccess => _validationErrors.Count == 0;
    public bool IsFailed  => _validationErrors.Count != 0;

    public static implicit operator Result<T?>(Result result) => new(default(T))
    {
        _validationErrors = result._validationErrors
    };

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failed(params ValidationError[] validationErrors) => new(validationErrors);
    public static Result<T> Failed(string error, string errorCode) => new(error, errorCode);
}