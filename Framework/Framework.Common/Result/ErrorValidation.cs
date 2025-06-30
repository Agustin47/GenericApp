namespace Framework.Common.Result;

public class ErrorValidation(string errorMessage, string errorCode, ErrorCause[] causes)
{
    public ErrorValidation(string errorMessage, string errorCode) : this(errorMessage, errorCode, []){}
    public string ErrorMessage { get; set; } = errorMessage;
    public string ErrorCode { get; set; } = errorCode;
    public ErrorCause[] Causes { get; set; } = causes;
}