using Application.Roles;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Security;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserCreation;

public class CreateUserCmdHandler(ISecurityService securityService, ILogger<CreateUserCmdHandler> logger) : ICommandHandler<CreateUserCmd>
{
    public async Task<Result> Handle(CreateUserCmd command)
    {
        var appRole = Rol.GetByName(command.Role);
        if(appRole == null)
            return Result.Failed(ExpectedErrors.Generic("Role not found"));
        
        var permissionsName = appRole.Permissions.Select(x => x.Name).ToArray();
        var registerUserResult = await securityService.RegisterUser(command.Username, command.Password, command.Email, command.Name, command.LastName, command.Role, permissionsName);
        if(registerUserResult.IsFailed)
            return registerUserResult;

        return Result.Success();
    }
}