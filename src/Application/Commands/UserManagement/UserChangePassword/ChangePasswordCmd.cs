using Framework.CQRS.Implementation;

namespace Application.Commands.UserManagement.UserChangePassword;

public class ChangePasswordCmd : CommandBase
{
    public string Username { get; set; }
    public string NewPassword { get; set; }
}