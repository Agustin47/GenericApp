using Domain;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Database;
using Framework.Domain;
using Framework.Specification;

namespace Application.Commands.Presupuesto.Create;

public class CreatePresupuestoCmdHandler(IDomainEntityFactory domainEntityFactory, IRepositoryFactory repositoryFactory)
    : ICommandHandler<CreatePresupuestoCmd>
{
    public async Task<Result> Handle(CreatePresupuestoCmd command)
    {
        var presupuestoRepo = repositoryFactory.GetRepository<Domain.Aggregates.Presupuesto>();
        var queryRepo = QueryRepositoryBuilder<Domain.Aggregates.Presupuesto>.Create()
            .AddSpecs(Specification<Domain.Aggregates.Presupuesto>.Create(x => x.Mes == command.Mes && x.Año == command.Año))
            .Build();
        
        var exist = await presupuestoRepo.FirstOrDefault(queryRepo);
        if(exist.Value != null)
            return Result.Failed(ExpectedErrors.Generic($"Presupuesto ya existe para el período {exist.Value.FechaAplica:MM/yyyy}"));
        
        EntityId entityId = new(Guid.NewGuid());
        var presupuesto = domainEntityFactory.Create<Domain.Aggregates.Presupuesto>(entityId);
        presupuesto.New(command.Comuna, command.ComunaId, command.Planificado, command.Mes, command.Año, 
            command.UserContext.Username);;
        
        await presupuesto.SaveChanges();
        return Result.Success();
    }
}