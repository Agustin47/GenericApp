namespace Framework.Security.Role;

public abstract class PermissionBase(string name, string describe)
{
    public string Name { get; private set; } = name;
    public string Describe { get; private set; } = describe;
    
    public static bool operator ==(PermissionBase x, PermissionBase y) => x.Name == y.Name;
    public static bool operator !=(PermissionBase x, PermissionBase y) => x.Name != y.Name;

    public override bool Equals(object obj)
    {
        var other = obj as PermissionBase;
        if (other == null) return false;
        return this == other;
    }
}