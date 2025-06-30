using Framework.Common.Result;
using Framework.CQRS.Commands;

namespace Framework.CQRS.Implementation;

public abstract class CommandBasePermissionValidator<TCommand> : ICommandPermissionValidator<TCommand> where TCommand : CommandBase
{
    protected abstract string Permission { get; }
    
    public virtual Result ValidatePermission(TCommand command)
        => command.UserContext.Permissions.Contains(Permission)
            ? Result.Success()
            : Result.Failed(ExpectedErrors.UnAuthorized);
}