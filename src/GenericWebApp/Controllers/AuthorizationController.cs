using Framework.Security;
using GenericWebApp.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebApp.Controllers;

[ApiController]
[AllowAnonymous]
[Route("Auth")]
public class AuthorizationController(ISecurityService securityService, ILogger<AuthorizationController> logger)
    : ControllerBase
{

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLogin user)
    {
        var loginResult = securityService.Login(user.Username, user.Password);
        
        if(loginResult.IsFailed)
            return Unauthorized("Username or password are incorrect");
        
        return Ok(loginResult.Value);
    }
    
    [HttpPost("refresh-token")]
    public IActionResult RefreshToken([FromBody] Refresh token)
    {
        var refreshTokenResult = securityService.RefreshToken(token.Token, token.RefreshToken);
        
        if(refreshTokenResult.IsFailed)
            return Unauthorized("Token is invalid");
        
        return Ok(refreshTokenResult.Value);
    }
    
    [HttpPost("logout")]
    public IActionResult Logout([FromBody] string username)
    {
        var logoutResult = securityService.Logout(username);

        if (logoutResult.IsFailed)
            return BadRequest("Something went wrong");
        
        return Ok();
    }
}