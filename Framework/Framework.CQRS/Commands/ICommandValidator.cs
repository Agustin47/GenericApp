namespace Framework.CQRS.Commands;

public interface ICommandValidator<TCommand> where TCommand : ICommand
{
    bool Validate(TCommand command);
}