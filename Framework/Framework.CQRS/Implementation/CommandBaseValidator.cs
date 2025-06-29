using Framework.Common.Result;
using Framework.CQRS.Commands;

namespace Framework.CQRS.Implementation;

public class CommandBaseValidator<TCommand> : ICommandValidator<TCommand> where TCommand : ICommand
{
    
    public Result Validate(TCommand command)
    {
        throw new NotImplementedException();
    }
}