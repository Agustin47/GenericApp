using Framework.Security;
using GenericWebApp.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class UserManagementController(ISecurityService securityService, ILogger<AuthorizationController> logger)
    : ControllerBase
{
    [HttpPost]
    public IActionResult CreateUser([FromBody] UMCreateUser User)
    {
        var registerUserResult = securityService.RegisterUser(User.Username, User.Password, User.Email, User.Name, User.LastName);
        if(registerUserResult.IsFailed)
            return BadRequest(registerUserResult.ValidationErrors);;
        
        return Ok();
    }
    
    [HttpPost("change-password")]
    public IActionResult ChangePassword([FromBody] UMChangePassword changePassword)
    {
        var changePasswordResult = securityService.ChangePassword(changePassword.Username, changePassword.NewPassword);
        if(changePasswordResult.IsFailed)
            return BadRequest(changePasswordResult.ValidationErrors);;
        
        return Ok();
    }
}