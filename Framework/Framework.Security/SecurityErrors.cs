using Framework.Common.Result;

namespace Framework.Security;

public static class SecurityErrors
{
    public static ValidationError LoginFailed = new("Login failed", "SEC-001");
    public static ValidationError RefreshTokenFailed = new("Refreshing token failed", "SEC-002");
}