using Domain;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Domain;

namespace Application.Commands.Solicitud.CancelSolicitud;

public class CancelSolicitudCmdHandler(IDomainEntityFactory domainEntityFactory)
    :  ICommandHandler<CancelSolicitudCmd>
{
    public async Task<Result> Handle(CancelSolicitudCmd command)
    {
        EntityId solicitudId = new(command.EntityId);
        var solicitud = await domainEntityFactory.GetByIdAsync<Domain.Aggregates.Solicitud, EntityId>(solicitudId);
        
        if(solicitudId == null)
            return Result.Failed(ExpectedErrors.Generic("No existe la solicitud"));
        
        EntityId presupuestoId = new(solicitud.PresupuestoId);
        var presupuesto = await domainEntityFactory.GetByIdAsync<Domain.Aggregates.Presupuesto, EntityId>(presupuestoId);
        
        if(presupuesto == null)
            return Result.Failed(ExpectedErrors.Generic("Error inesperado, consultar con IT: No existe el presupuesto"));
        
        var result = solicitud.Cancel(command.UserContext.Username);
        if(result.IsFailed)
            return result;
        
        presupuesto.RemoveGastado(solicitudId.Value, command.UserContext.Username);
        
        await solicitud.SaveChanges();
        await presupuesto.SaveChanges();
        
        return Result.Success();
        
    }
}