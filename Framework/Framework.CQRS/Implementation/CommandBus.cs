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
            if(_serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) is not ICommandHandler<TCommand> handler)
                throw new Exception("Handler not found");

            if (_serviceProvider.GetService(typeof(ICommandValidator<TCommand>)) is ICommandValidator<TCommand> commandValidator)
            {
                var validationResult = commandValidator.ValidateCommand(command);
                if (validationResult.IsFailed)
                    return validationResult;
            }

            if (_serviceProvider.GetService(typeof(ICommandPermissionValidator<TCommand>)) is ICommandPermissionValidator<TCommand> permissionValidator)
            {
                var permissionResult = permissionValidator.ValidatePermission(command);
                if (permissionResult.IsFailed)
                    return permissionResult;
            }
            
            return await handler.Handle(command);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}