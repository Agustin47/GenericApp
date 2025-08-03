using Application.Commands.Solicitud.ApproveSolicitud;
using Application.Commands.Solicitud.CancelSolicitud;
using Application.Commands.Solicitud.CreateSolicitud;
using Application.Queries.GetSolicitud;
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
public class SolicitudController(ICommandBus commandBus, IQueryBus queryBus, ISecurityService securityService, ILogger<AuthorizationController> logger)
    : GenericControllerBase(securityService)
{
  
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSolicitud solicitud)
    {

        CreateSolicitudCmd cmd = new()
        {
            SolicitanteId = solicitud.SolicitanteId,
            TipoAyuda = solicitud.TipoAyuda,
            Detalles = solicitud.Detalles,
            ComunaId = solicitud.ComunaId,
            Comuna = solicitud.Comuna,
            Monto = solicitud.Monto,
            Estado = solicitud.Estado,
            Gestor = solicitud.Gestor,
            Urgencia = solicitud.Urgencia,
            Justificacion = solicitud.Justificacion,
            Observaciones = solicitud.Observaciones,
            UserContext = GetUserContext(),
        };
        
        var createSolicitudResult = await commandBus.Handle(cmd);
        if (createSolicitudResult.IsFailed)
            return BadRequest(createSolicitudResult);
        
        return Ok();
    }

    [HttpPost("approve/{id}")]
    public async Task<IActionResult> Approve([FromRoute] Guid id)
    {
        ApproveSolicitudCmd cmd = new()
        {
            EntityId = id,
            UserContext = GetUserContext(),
        };
        
        var approveSolicitud = await commandBus.Handle(cmd);
        if (approveSolicitud.IsFailed)
            return BadRequest(approveSolicitud);
        
        return Ok();
    }
    
    [HttpPost("cancel/{id}")]
    public async Task<IActionResult> Cancel([FromRoute] Guid id)
    {
        CancelSolicitudCmd cmd = new()
        {
            EntityId = id,
            UserContext = GetUserContext(),
        };
        
        var cancelSolicitud = await commandBus.Handle(cmd);
        if (cancelSolicitud.IsFailed)
            return BadRequest(cancelSolicitud);
        
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(QueryBaseModel? query)
    {
        GetSolicitudQuery get = new()
        {
            UserContext = GetUserContext(),
            Filters = query?.Filters,
            Sorting = query?.Sorting,
            Paging = query?.Paging,
        };

        var persons = await queryBus.Handle<List<Solicitud>>(get);
        
        return Ok(persons);
    }
}