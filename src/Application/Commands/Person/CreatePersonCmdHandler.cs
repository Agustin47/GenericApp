using Domain;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Database;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Person;

public class CreatePersonCmdHandler(IRepositoryFactory repositoryFactory, ILogger<CreatePersonCmdHandler> logger) : ICommandHandler<CreatePersonCmd>
{
    private readonly IRepository<Domain.Person> _personRepository = repositoryFactory.GetRepository<Domain.Person>();
    
    public async Task<Result> Handle(CreatePersonCmd command)
    {
        
        PersonId personId = new(Guid.NewGuid());
        Domain.Person person = new(personId);
        
        person.Register(command.Identification, command.Name, command.LastName, command.Age, command.UserContext.Username);

        await _personRepository.CreateAsync(person);
        
        return Result.Success();
    }
}