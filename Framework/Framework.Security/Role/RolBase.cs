namespace Framework.Security.Role;

public abstract class RolBase(string name, string description, PermissionBase[] permissions)
{
    protected string Name { get; private set; } = name;
    protected string Description { get; private set; } = description;
    protected PermissionBase[] Permissions { get; private set; } = permissions;
}