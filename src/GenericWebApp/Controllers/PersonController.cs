using Application.Commands.Person.Create;
using Application.Commands.Person.Update;
using Application.Queries.GetPerson;
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
public class PersonController(ICommandBus commandBus, IQueryBus queryBus, ISecurityService securityService, ILogger<AuthorizationController> logger)
    : GenericControllerBase(securityService)
{

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonCreate person)
    {
        CreatePersonCmd command = new()
        {
            Dni = person.Dni,
            Nombre = person.Nombre,
            Apellido = person.Apellido,
            Telefono = person.Telefono,
            Email = person.Email,
            Direccion = person.Direccion,
            Comuna = person.Comuna,
            Barrio = person.Barrio,
            FechaNacimiento = person.FechaNacimiento,
            AnioNacimiento = person.AnioNacimiento,
            TipoDocumento = person.TipoDocumento,
            Genero = person.Genero,
            EstadoCivil = person.EstadoCivil,
            Ocupacion = person.Ocupacion,
            IngresosFamiliares = person.IngresosFamiliares,
            NumeroFamiliares = person.NumeroFamiliares,
            TipoVivienda = person.TipoVivienda,
            ServiciosBasicos = person.ServiciosBasicos,
            DechaRegistro = person.DechaRegistro,
            UltimaActualizacion = person.UltimaActualizacion,
            Estado = person.Estado,
            TotalSolicitudes = person.TotalSolicitudes,
            MontoTotalRecibido = person.MontoTotalRecibido,
            UltimaSolicitud = person.UltimaSolicitud,
            Observaciones = person.Observaciones,
            Obs = person.Obs,
            UserContext = GetUserContext(),
        };
        
        var createPersonResult = await commandBus.Handle(command);
        if (createPersonResult.IsFailed)
            return BadRequest(createPersonResult);

        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCreate person)
    {
        UpdatePersonCmd command = new()
        {
            Id = person.Id,
            Dni = person.Dni,
            Nombre = person.Nombre,
            Apellido = person.Apellido,
            Telefono = person.Telefono,
            Email = person.Email,
            Direccion = person.Direccion,
            Comuna = person.Comuna,
            Barrio = person.Barrio,
            FechaNacimiento = person.FechaNacimiento,
            AnioNacimiento = person.AnioNacimiento,
            TipoDocumento = person.TipoDocumento,
            Genero = person.Genero,
            EstadoCivil = person.EstadoCivil,
            Ocupacion = person.Ocupacion,
            IngresosFamiliares = person.IngresosFamiliares,
            NumeroFamiliares = person.NumeroFamiliares,
            TipoVivienda = person.TipoVivienda,
            ServiciosBasicos = person.ServiciosBasicos,
            DechaRegistro = person.DechaRegistro,
            UltimaActualizacion = person.UltimaActualizacion,
            Estado = person.Estado,
            TotalSolicitudes = person.TotalSolicitudes,
            MontoTotalRecibido = person.MontoTotalRecibido,
            UltimaSolicitud = person.UltimaSolicitud,
            Observaciones = person.Observaciones,
            Obs = person.Obs,
            UserContext = GetUserContext(),
        };
        
        var createPersonResult = await commandBus.Handle(command);
        if (createPersonResult.IsFailed)
            return BadRequest(createPersonResult);

        return Ok();
    }


    [HttpGet]
    public async Task<IActionResult> Get(QueryBaseModel? query)
    {
        GetPersonQuery get = new()
        {
            UserContext = GetUserContext(),
            Filters = query?.Filters,
            Sorting = query?.Sorting,
            Paging = query?.Paging,
        };

        var persons = await queryBus.Handle<List<Persona>>(get);
        
        return Ok(persons);
    }

}