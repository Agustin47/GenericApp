using Application.Commands.Presupuesto.Create;
using Application.Queries.GetPresupuesto;
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
public class PresupuestoController(ICommandBus commandBus, IQueryBus queryBus, ISecurityService securityService, ILogger<AuthorizationController> logger)
    : GenericControllerBase(securityService)
{
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePresupuesto presupuesto)
    {
        CreatePresupuestoCmd cmd = new()
        {
            Comuna = presupuesto.Comuna,
            ComunaId = presupuesto.ComunaId,
            Planificado = presupuesto.Planificado,
            Mes = presupuesto.Mes,
            UserContext = GetUserContext(),
        };
        
        var createPresupuestoResult = await commandBus.Handle(cmd);
        if (createPresupuestoResult.IsFailed)
            return BadRequest();

        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(QueryBaseModel? query)
    {
        GetPresupuestoQuery get = new()
        {
            UserContext = GetUserContext(),
            Filters = query?.Filters,
            Sorting = query?.Sorting,
            Paging = query?.Paging,
        };

        var persons = await queryBus.Handle<List<Presupuesto>>(get);
        
        return Ok(persons);
    }
    
}