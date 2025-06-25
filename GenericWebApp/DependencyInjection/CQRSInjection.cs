using System.Reflection;
using Application.Commands.Weather;
using Framework.CQRS;
using Framework.CQRS.Commands;
using Framework.CQRS.Implementation;

namespace GenericWebApp.DependencyInjection;

public static class CQRSInjection
{
    public static IServiceCollection AddCQRS(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.Load("Application");

        
        var commands = assembly.DefinedTypes
            .Where(x => x.ImplementedInterfaces.Contains(typeof(ICommand)));

        foreach (var command in commands)
        {
            
            var handler = assembly.DefinedTypes
                .FirstOrDefault(x => x.ImplementedInterfaces.Contains(typeof(ICommandHandler<>)));
            
        }

        services.AddTransient<ICommandHandler<WeatherCommand>, WeatherCommandHandler>();
        services.AddTransient<ICommandBus, CommandBus>();
        
        
        return services;
    }
}