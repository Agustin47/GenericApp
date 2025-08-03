using Application.Commands.Comuna.Create;
using Application.Queries.GetComuna;
using Domain.Aggregates;
using Framework.CQRS.Commands;
using Framework.CQRS.Queries;
using Framework.Security;
using GenericWebApp.Models;
using GenericWebApp.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebApp.Controllers;


[ApiController]
[Route("[controller]")]
public class ComunaController(
    ICommandBus commandBus,
    IQueryBus queryBus,
    ISecurityService securityService,
    ILogger<AuthorizationController> logger)
    : GenericControllerBase(securityService)
{

    [HttpPost]
    public async Task<IActionResult> CreateComuna([FromBody] CreateComuna comuna)
    {
        CreateComunaCmd cmd = new()
        {
            Nombre = comuna.Nombre,
            Codigo = comuna.Codigo,
            Estado = comuna.Estado,
            Poblacion = comuna.Poblacion,
            OficinasId = comuna.OficinasId,
            UserContext = GetUserContext(),
        };
        
        var result = await commandBus.Handle(cmd);
        if (result.IsFailed)
            return BadRequest(result);
        
        return Ok();
    }
    
    
    
    [HttpGet]
    public async Task<IActionResult> Get(QueryBaseModel? query)
    {
        GetComunaQuery get = new()
        {
            UserContext = GetUserContext(),
            Filters = query?.Filters,
            Sorting = query?.Sorting,
            Paging = query?.Paging,
        };

        var comunas = await queryBus.Handle<List<Comuna>>(get);
        
        return Ok(comunas);
    }
}