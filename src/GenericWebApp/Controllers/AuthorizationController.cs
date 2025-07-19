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
    public async Task<IActionResult> Login([FromBody] UserLogin user)
    {
        var loginResult = await securityService.Login(user.Username, user.Password);
        
        if(loginResult.IsFailed)
            return Unauthorized("Username or password are incorrect");
        
        return Ok(loginResult.Value);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] Refresh token)
    {
        var refreshTokenResult = await securityService.RefreshToken(token.Token, token.RefreshToken);
        
        if(refreshTokenResult.IsFailed)
            return Unauthorized("Token is invalid");
        
        return Ok(refreshTokenResult.Value);
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string username)
    {
        var logoutResult = await securityService.Logout(username);

        if (logoutResult.IsFailed)
            return BadRequest("Something went wrong");
        
        return Ok();
    }
}