using Domain;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Domain;

namespace Application.Commands.Solicitud.ApproveSolicitud;

public class ApproveSolicitudCmdHandler(IDomainEntityFactory domainEntityFactory)
    : ICommandHandler<ApproveSolicitudCmd>
{
    public async Task<Result> Handle(ApproveSolicitudCmd command)
    {
        EntityId solicitudId = new(command.EntityId);
        var solicitud = await domainEntityFactory.GetByIdAsync<Domain.Aggregates.Solicitud, EntityId>(solicitudId);
        
        if(solicitudId == null)
            return Result.Failed(ExpectedErrors.Generic("No existe la solicitud"));
        
        var result = solicitud.Approve(command.UserContext.Username);
        if(result.IsFailed)
            return result;
        await solicitud.SaveChanges();
        
        return Result.Success();
    }
}