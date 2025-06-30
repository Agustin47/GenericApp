using Framework.Common.Result;

namespace Framework.Security;

public static class SecurityErrors
{
    public static ErrorValidation LoginFailed = new("Login failed", "SEC-001");
    public static ErrorValidation RefreshTokenFailed = new("Refreshing token failed", "SEC-002");
}