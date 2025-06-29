namespace Framework.Security;

public class UserContext
{
    public string Username { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}