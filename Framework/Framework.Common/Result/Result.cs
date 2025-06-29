namespace Framework.Common.Result;

public class Result<T>
{
    private readonly List<ValidationError> _validationErrors;
    private readonly bool _isSuccess;
    public T? Value { get; private set; }

    protected Result()
    {
        Value = default;
        _isSuccess = true;
        _validationErrors = new List<ValidationError>();       
    }
    
    private Result(T? value)
    {
        Value = value;
        _isSuccess = true;
        _validationErrors = new List<ValidationError>();
    }

    protected Result(params ValidationError[] validationErrors)
    {
        Value = default;
        _isSuccess = false;
        _validationErrors = new List<ValidationError>(validationErrors);
    }

    protected Result(string error, string errorCode)
    {
        Value = default;
        _isSuccess = false;
        _validationErrors = [new ValidationError(error, errorCode)];
    }

    public IReadOnlyCollection<ValidationError> ValidationErrors => _validationErrors.AsReadOnly();

    public bool IsFailed  => !_isSuccess;

    public static implicit operator Result<T?>(Result result)
    {
        if (result.IsFailed)
            return new(result._validationErrors.ToArray());        
        return new(default(T));
    }

    protected static Result<T> Success(T value) => new(value);
    public static Result<T> Failed(params ValidationError[] validationErrors) => new(validationErrors);
    public static Result<T> Failed(string error, string errorCode) => new(error, errorCode);
}