using Framework.Common.Result;
using Framework.CQRS.Commands;
using Microsoft.Extensions.Logging;

namespace Framework.CQRS.Implementation;

public class CommandBus : ICommandBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CommandBus> _logger;

    public CommandBus(IServiceProvider serviceProvider, ILogger<CommandBus> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task<Result> Handle<TCommand>(TCommand command) where TCommand : ICommand
    {
        try
        {
            _logger.LogInformation("Handling command {CommandName}", typeof(TCommand).Name);
            _logger.LogInformation($"Command: {System.Text.Json.JsonSerializer.Serialize(command)}");
            if(_serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) is not ICommandHandler<TCommand> handler)
                throw new Exception("Handler not found");

            if (_serviceProvider.GetService(typeof(ICommandValidator<TCommand>)) is ICommandValidator<TCommand> commandValidator)
            {
                _logger.LogInformation("Executing command validator");
                var validationResult = commandValidator.ValidateCommand(command);
                if (validationResult.IsFailed)
                    return validationResult;
            }

            if (_serviceProvider.GetService(typeof(ICommandPermissionValidator<TCommand>)) is ICommandPermissionValidator<TCommand> permissionValidator)
            {
                _logger.LogInformation("Executing command permission validator");
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