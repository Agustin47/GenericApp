using Framework.Common.Result;
using Framework.CQRS.Commands;

namespace Application.Commands.Comuna.Create;

public class CreateComunaCmdHandler : ICommandHandler<CreateComunaCmd>
{
    public Task<Result> Handle(CreateComunaCmd command)
    {
        throw new NotImplementedException();
    }
}