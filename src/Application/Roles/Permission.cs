using Framework.Security.Role;

namespace Application.Roles;

public class Permission : PermissionBase
{
    public static readonly Permission UserModify = new("user:modify", "Allow to create/edit user");
    public static readonly Permission UserGet = new("user:get", "Allow to get users");
    
    public static readonly Permission SolicitationModify = new("solicitation:modify", "Allow to create/edit solicitation");
    public static readonly Permission SolicitationGet = new("solicitation:get", "Allow to get solicitations");
    public static readonly Permission SolicitationApprove = new("solicitation:approve", "Allow to approve solicitations");

    private Permission(string name, string describe) : base(name, describe){}
}