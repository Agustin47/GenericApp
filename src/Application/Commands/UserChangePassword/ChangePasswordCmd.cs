using Framework.CQRS.Implementation;

namespace Application.Commands.UserChangePassword;

public class ChangePasswordCmd : CommandBase
{
    public string Username { get; set; }
    public string NewPassword { get; set; }
}