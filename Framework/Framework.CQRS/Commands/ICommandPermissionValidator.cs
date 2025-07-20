using Framework.Common.Result;

namespace Framework.CQRS.Commands;

public interface ICommandPermissionValidator<in TCommand> where TCommand : ICommand
{
    Result ValidatePermission(TCommand command);
}