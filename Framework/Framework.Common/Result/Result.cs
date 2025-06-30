namespace Framework.Common.Result;

public class Result<T>
{
    private readonly List<ErrorValidation> _validationErrors;
    private readonly bool _isSuccess;
    public T? Value { get; private set; }

    protected Result()
    {
        Value = default;
        _isSuccess = true;
        _validationErrors = new List<ErrorValidation>();       
    }
    
    private Result(T? value)
    {
        Value = value;
        _isSuccess = true;
        _validationErrors = new List<ErrorValidation>();
    }

    protected Result(params ErrorValidation[] validationErrors)
    {
        Value = default;
        _isSuccess = false;
        _validationErrors = new List<ErrorValidation>(validationErrors);
    }

    protected Result(string error, string errorCode)
    {
        Value = default;
        _isSuccess = false;
        _validationErrors = [new ErrorValidation(error, errorCode)];
    }

    public IReadOnlyCollection<ErrorValidation> ValidationErrors => _validationErrors.AsReadOnly();

    public bool IsFailed  => !_isSuccess;

    public static implicit operator Result<T?>(Result result)
    {
        if (result.IsFailed)
            return new(result._validationErrors.ToArray());        
        return new(default(T));
    }

    protected static Result<T> Success(T value) => new(value);
    public static Result<T> Failed(params ErrorValidation[] validationErrors) => new(validationErrors);
    public static Result<T> Failed(string error, string errorCode) => new(error, errorCode);
}