using Application.Queries.GetComuna;
using Domain.Aggregates;
using Framework.CQRS.Commands;
using Framework.CQRS.Queries;
using Framework.Security;
using GenericWebApp.Models;
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