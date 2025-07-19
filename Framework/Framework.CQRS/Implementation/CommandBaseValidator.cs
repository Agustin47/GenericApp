using FluentValidation;
using Framework.Common.Result;
using Framework.CQRS.Commands;

namespace Framework.CQRS.Implementation;

public abstract class CommandBaseValidator<TCommand> : AbstractValidator<TCommand>, ICommandValidator<TCommand> where TCommand : CommandBase
{
    public Result ValidateCommand(TCommand command)
    {
        var fluentResult = Validate(command);
        if (fluentResult.IsValid)
            return Result.Success();

        var causes = fluentResult.Errors.Select(x => new ErrorCause(x.PropertyName, x.ErrorMessage)).ToArray();
        return Result.Failed(ExpectedErrors.Validation(causes));
    }
}