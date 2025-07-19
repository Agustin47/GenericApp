using Domain;
using Application.Commands.Person;
using Application.Queries.GetPerson;
using Framework.CQRS.Commands;
using Framework.CQRS.Queries;
using Framework.Security;
using GenericWebApp.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GenericWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController(ICommandBus commandBus, IQueryBus queryBus, ISecurityService securityService, ILogger<AuthorizationController> logger)
    : _Base(securityService)
{

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonCreate person)
    {
        CreatePersonCmd command = new()
        {
            Identification = person.Identification,
            Name = person.Name,
            LastName = person.LastName,
            Age = person.Age,
            UserContext = GetUserContext(),
        };
        
        var createPersonResult = await commandBus.Handle(command);
        if (createPersonResult.IsFailed)
            return BadRequest();

        return Ok();
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        GetPersonQuery query = new()
        {
            UserContext = GetUserContext(),
        };

        var persons = await queryBus.Handle<Person>(query);
        
        return Ok();
    }

}