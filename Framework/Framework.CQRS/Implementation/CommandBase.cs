using Framework.CQRS.Commands;
using Framework.Security;

namespace Framework.CQRS.Implementation;

public abstract class CommandBase : ICommand
{
    public required UserContext UserContext { get; set; }
}