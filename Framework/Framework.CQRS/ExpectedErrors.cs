using Framework.Common.Result;

namespace Framework.CQRS;

public class ExpectedErrors
{
    private const string ErrorCodePrefix = "FW-CQRS";
    public static ErrorValidation UnAuthorized = new("UnAuthorized to perform this action", $"{ErrorCodePrefix}-001");
}