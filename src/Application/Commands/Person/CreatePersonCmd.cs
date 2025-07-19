using Framework.CQRS.Implementation;

namespace Application.Commands.Person;

public class CreatePersonCmd : CommandBase
{
    public string Identification { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}