using Framework.Common.Result;
using Framework.CQRS.Commands;

namespace Application.Commands.Comuna.Update;

public class UpdateComunaCmdHandler : ICommandHandler<UpdateComunaCmd>
{
    public Task<Result> Handle(UpdateComunaCmd command)
    {
        throw new NotImplementedException();
    }
}