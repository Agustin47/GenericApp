using Framework.Common.Result;

namespace Application;

public class ExpectedErrors
{
    private const string ErrorCodePrefix = "APP";
    
    public static ErrorValidation UnAuthorized = new("Unauthorized to perform this action", $"{ErrorCodePrefix}-001");
    public static ErrorValidation Generic(string error) => new(error, $"{ErrorCodePrefix}-000");
}