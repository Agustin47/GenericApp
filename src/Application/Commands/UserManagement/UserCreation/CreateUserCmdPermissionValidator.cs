using Framework.CQRS.Implementation;

namespace Application.Commands.UserManagement.UserCreation;

public class CreateUserCmdPermissionValidator : CommandBasePermissionValidator<CreateUserCmd>
{
    protected override string Permission => Roles.Permission.UserModify.Name;
}