using FluentValidation;
using Framework.Common.Result;
using Framework.CQRS.Commands;

namespace Framework.CQRS.Implementation;

public abstract class CommandBaseValidator<TCommand> : AbstractValidator<TCommand>, ICommandValidator<TCommand> where TCommand : CommandBase
{
    public Result ValidateCommand(TCommand command)
    {
        var fluentResult = Validate(command);
        return Result.Success();
    }
}