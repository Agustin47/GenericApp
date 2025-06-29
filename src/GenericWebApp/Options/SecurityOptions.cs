using Framework.Security;

namespace GenericWebApp.Options;

public class SecurityOptions : ISecurityOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpireInMinutes { get; set; }
    public string Secret { get; set; }
}