using Framework.Common.Result;

namespace Application;

public class ExpectedErrors
{
    private const string ErrorCodePrefix = "APP";
    public static ErrorValidation UnAuthorized = new("UnAuthorized to perform this action", $"{ErrorCodePrefix}-001");
}