namespace Framework.Security.Role;

public abstract class PermissionBase(string name, string describe)
{
    public string Name { get; private set; } = name;
    public string Describe { get; private set; } = describe;
}