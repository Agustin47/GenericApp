using Framework.CQRS.Commands;
using Framework.Security;

namespace Framework.CQRS.Implementation;

public class CommandBase : ICommand
{
    public UserContext UserContext { get; set; }
}