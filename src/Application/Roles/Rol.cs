using Framework.Security.Role;

namespace Application.Roles;

public class Rol : RolBase
{
    public static readonly Rol UserManager = new("UserManager", "Rol to allow the manager to create/edit users", new[] { Permission.UserModify, Permission.UserGet });
    public static readonly Rol SolicitationManager = new("SolicitationManager", "Rol to allow the manager to create/edit/approve solicitations", new[] { Permission.SolicitationModify, Permission.SolicitationGet, Permission.SolicitationApprove });
    public static readonly Rol SolicitationUser = new("SolicitationUser", "Rol to allow the user to create/edit solicitations but approve", new[] { Permission.SolicitationModify, Permission.SolicitationGet });

    private Rol(string name, string description, Permission[] permissions) : base(name, description, permissions){}
}