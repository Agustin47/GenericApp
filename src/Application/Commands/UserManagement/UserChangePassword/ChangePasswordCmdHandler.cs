using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Security;
using Microsoft.Extensions.Logging;

namespace Application.Commands.UserManagement.UserChangePassword;

public class ChangePasswordCmdHandler(ISecurityService securityService, ILogger<ChangePasswordCmdHandler> logger) : ICommandHandler<ChangePasswordCmd>
{
    public async Task<Result> Handle(ChangePasswordCmd command)
    {
        var changePasswordResult = await securityService.ChangePassword(command.Username, command.NewPassword);
        if(changePasswordResult.IsFailed)
            return changePasswordResult;
        
        return Result.Success();
    }
}