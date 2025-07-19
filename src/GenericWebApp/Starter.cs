using Application.Roles;
using Framework.Security;

namespace GenericWebApp;

public class AutomaticStarter(ISecurityService securityService, ILogger<AutomaticStarter> logger) : IHostedService //BackgroundService
{
    private const string Username = "admin";
    private const string Password = "admin";
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var login = await securityService.Login(Username, Password);
        if (!login.IsFailed)
            return;

        var newUserRoles = Rol.UserManager.Permissions.Select(x => x.Name).ToArray();
        await securityService.RegisterUser(Username, Password, string.Empty, Username, Username,
            Rol.UserManager.Name, newUserRoles);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}