using Application.Commands.UserManagement.UserChangePassword;
using Application.Commands.UserManagement.UserCreation;
using Application.Roles;
using Framework.CQRS.Commands;
using Framework.Security;
using GenericWebApp.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class UserManagementController(ICommandBus commandBus, ISecurityService securityService, ILogger<AuthorizationController> logger)
    : GenericControllerBase(securityService)
{

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UmCreateUser User)
    {
        CreateUserCmd command = new()
        {
            Username = User.Username,
            Password = User.Password,
            Email = User.Email,
            Name = User.Name,
            LastName = User.LastName,
            Role = User.Role,
            UserContext = GetUserContext(),
        };
        var createUserResult = await commandBus.Handle(command);
        if (createUserResult.IsFailed)
            return BadRequest(createUserResult);
        
        return Ok();
    }
    
    [HttpPost("change-password")]
    public IActionResult ChangePassword([FromBody] UmChangePassword changePassword)
    {
        ChangePasswordCmd command = new()
        {
            Username = changePassword.Username,
            NewPassword = changePassword.NewPassword,
            UserContext = GetUserContext(),
        };
        var createUserResult = commandBus.Handle(command);
        if (createUserResult.IsFaulted)
            return BadRequest(createUserResult);
        
        return Ok();
    }
    
    [HttpGet("roles")]
    public IActionResult GetRoles()
    {
        return Ok(Rol.GetAll());
    }
}