using Domain;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Domain;

namespace Application.Commands.Presupuesto.Edit;

public class EditPresupuestoCmdHandler(IDomainEntityFactory domainEntityFactory)
    : ICommandHandler<EditPresupuestoCmd>
{
    public async Task<Result> Handle(EditPresupuestoCmd command)
    {
        
        EntityId id = new(command.EntityId);
        var presupuesto = await domainEntityFactory.GetByIdAsync<Domain.Aggregates.Presupuesto, EntityId>(id);

        if(presupuesto == null)
            return Result.Failed(ExpectedErrors.Generic("No existe el presupuesto"));
        
        var result = presupuesto.EditPresupuesto(command.Planificado, command.UserContext.Username);
        if(result.IsFailed)
            return result;
        await presupuesto.SaveChanges();

        return Result.Success();
    }
}