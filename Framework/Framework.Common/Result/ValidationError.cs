namespace Framework.Common.Result;

public class ValidationError(string errorMessage, string errorCode)
{
    public string ErrorMessage { get; set; } = errorMessage;
    public string ErrorCode { get; set; } = errorCode;
}