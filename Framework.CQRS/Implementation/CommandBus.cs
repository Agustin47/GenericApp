using Framework.Common.Result;
using Framework.CQRS.Commands;

namespace Framework.CQRS.Implementation;

public class CommandBus : ICommandBus
{
    private readonly IServiceProvider _serviceProvider;

    public CommandBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<Result> Handle<TCommand>(TCommand command) where TCommand : ICommand
    {
        try
        {
            // get commandHandler which implement ICommandHandler<TCommand>
            if(_serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) is not ICommandHandler<TCommand> handler)
                throw new Exception("Handler not found");

            return await handler.Handle(command);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}