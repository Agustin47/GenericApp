namespace Framework.Security;

public class UserContext
{
    private readonly List<string> _permissions;
    internal UserContext(string username, List<string> permissions)
    {
        Username = username;
        _permissions = permissions;
    }

    public string Username { get; set; }
    public IEnumerable<string> Permissions => _permissions.AsReadOnly();
}