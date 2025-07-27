using Application.Queries.GetOficina;
using Domain.Aggregates;
using Framework.CQRS.Commands;
using Framework.CQRS.Queries;
using Framework.Security;
using GenericWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class OficinaController(ICommandBus commandBus, IQueryBus queryBus, ISecurityService securityService, ILogger<AuthorizationController> logger)
    : GenericControllerBase(securityService)
{
    
    
    [HttpGet]
    public async Task<IActionResult> Get(QueryBaseModel? query)
    {
        GetOficinaQuery get = new()
        {
            UserContext = GetUserContext(),
            Filters = query?.Filters,
            Sorting = query?.Sorting,
            Paging = query?.Paging,
        };

        var persons = await queryBus.Handle<List<Oficina>>(get);
        
        return Ok(persons);
    }
}