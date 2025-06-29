namespace Framework.Security;

public interface ISecurityOptions
{
    string Issuer { get; set; }
    string Audience { get; set; }
    int ExpireInMinutes { get; set; }
    string Secret { get; set; }
}