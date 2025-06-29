using Application.Queries.GetUser;
using Domain;
using Framework.CQRS.Commands;
using Framework.CQRS.Queries;
using Framework.Security;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ICommandBus commandBus, IQueryBus queryBus,
    ISecurityService securityService, ILogger<WeatherForecastController> logger) : ControllerBase
{

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        GetUserQuery query = new();
        var result = await queryBus.Handle<User>(query);
        
        return Ok();
    }
}