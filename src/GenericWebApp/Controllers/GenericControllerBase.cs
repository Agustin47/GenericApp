using Framework.Security;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebApp.Controllers;

public abstract class GenericControllerBase(ISecurityService securityService) : ControllerBase
{
    protected UserContext GetUserContext()
    {
        var token = Request.Headers.Authorization.ToString().Replace("Bearer ", string.Empty);
        var result = securityService.GetUserContext(token).GetAwaiter().GetResult();
        if(result.IsFailed || result.Value == null)
            throw new Exception("Invalid token");
        
        return result.Value;
    }
}