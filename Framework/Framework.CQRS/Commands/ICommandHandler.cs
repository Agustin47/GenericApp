using Framework.Common.Result;

namespace Framework.CQRS.Commands;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<Result> Handle(TCommand command);
}