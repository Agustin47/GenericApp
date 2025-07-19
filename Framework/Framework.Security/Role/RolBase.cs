namespace Framework.Security.Role;

public abstract class RolBase(string name, string description, PermissionBase[] permissions)
{
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public PermissionBase[] Permissions { get; private set; } = permissions;
    
    public static bool operator ==(RolBase x, RolBase y) => x?.Name == y?.Name;
    public static bool operator !=(RolBase x, RolBase y) => x?.Name != y?.Name;

    public override bool Equals(object obj)
    {
        var other = obj as RolBase;
        if (other == null) return false;
        return this == other;
    }
}