using Framework.Common.Result;

namespace Framework.CQRS.Commands;

public interface ICommandValidator<TCommand> where TCommand : ICommand
{
    Result ValidateCommand(TCommand command);
}