using Application.Commands.Weather;
using Application.Queries.GetUser;
using Domain;
using Framework.CQRS.Commands;
using Framework.CQRS.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ICommandBus _commandBus;
    private readonly IQueryBus _queryBus;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        ICommandBus commandBus,
        IQueryBus queryBus,
        ILogger<WeatherForecastController> logger)
    {
        _commandBus = commandBus;
        _queryBus = queryBus;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        WeatherCommand command = new();
        await _commandBus.Handle(command);
        
        GetUserQuery query = new();
        var result = await _queryBus.Handle<User>(query);
        
        return Ok();
    }
}