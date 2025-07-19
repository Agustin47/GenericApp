using Framework.CQRS.Implementation;

namespace Application.Commands.UserCreation;

public class CreateUserCmd : CommandBase
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
}
