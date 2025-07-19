using Framework.Common.Result;

namespace Framework.CQRS;

public class ExpectedErrors
{
    private const string ErrorCodePrefix = "FW-CQRS";
    public static ErrorValidation UnAuthorized = new("UnAuthorized to perform this action", $"{ErrorCodePrefix}-001");
    public static ErrorValidation Validation(ErrorCause[] causes) => new("Some validation have occur", $"{ErrorCodePrefix}-002", causes);
}