using Framework.Common.Result;

namespace Framework.CQRS.Commands;

public interface ICommandBus
{
    Task<Result> Handle<TCommand>(TCommand command) where TCommand : ICommand;
}