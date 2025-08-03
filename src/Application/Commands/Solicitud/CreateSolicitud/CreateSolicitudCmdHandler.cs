using Domain;
using Domain.Aggregates;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Database;
using Framework.Domain;
using Framework.Specification;

namespace Application.Commands.Solicitud.CreateSolicitud;

public class CreateSolicitudCmdHandler(IDomainEntityFactory domainEntityFactory, IRepositoryFactory repositoryFactory)
    : ICommandHandler<CreateSolicitudCmd>
{
    public async Task<Result> Handle(CreateSolicitudCmd command)
    {
        
        EntityId personaId = new(command.SolicitanteId);
        var persona = await domainEntityFactory.GetByIdAsync<Persona, EntityId>(personaId);
        if(persona == null)
            return Result.Failed(ExpectedErrors.Generic("No existe la persona"));

        EntityId comunaId = new(command.ComunaId);
        var comuna = await domainEntityFactory.GetByIdAsync<Domain.Aggregates.Comuna, EntityId>(comunaId);
        if(comuna == null)
            return Result.Failed(ExpectedErrors.Generic("No existe la comuna"));

        var presupuestoRepo = repositoryFactory.GetRepository<Domain.Aggregates.Presupuesto>();
        var query = QueryRepositoryBuilder<Domain.Aggregates.Presupuesto>.Create()
            .AddSpecs(Specification<Domain.Aggregates.Presupuesto>.Create(x => x.ComunaId == comuna.Id))
            .Build();
        
        var presupuestoResult = await presupuestoRepo.FirstOrDefault(query);
        
        if(presupuestoResult.Value == null)
            return Result.Failed(ExpectedErrors.Generic("No existe presupuesto para la comuna"));
        
        EntityId presupuestoId = new(presupuestoResult.Value.Id);
        var presupuesto = await domainEntityFactory.GetByIdAsync<Domain.Aggregates.Presupuesto, EntityId>(presupuestoId);
        
        var disponible = presupuesto.Gastado + command.Monto < presupuesto.Planificado;
        if(!disponible)
            return Result.Failed(ExpectedErrors.Generic($"No hay presupuesto disponible, gastado {presupuesto.Gastado} + {command.Monto} = {presupuesto.Gastado + command.Monto} > {presupuesto.Planificado}"));
        
        EntityId entityId = new(Guid.NewGuid());
        var solicitud = domainEntityFactory.Create<Domain.Aggregates.Solicitud>(entityId);
        solicitud.New(command.SolicitanteId, presupuesto.Id, command.TipoAyuda, command.Detalles, comuna.Id, comuna.Nombre,
            command.Monto, command.Urgencia, command.Justificacion, command.Observaciones,
            command.UserContext.Username);
        
        presupuesto.AddGastado(new()
            {
                Id = solicitud.Id,
                SolicitanteId = command.SolicitanteId,
                SolicitanteNombre = persona.Nombre,
                SolicitanteDNI = persona.Dni,
                Monto = command.Monto,
                
            }, command.UserContext.Username);

        await presupuesto.SaveChanges();
        await solicitud.SaveChanges();
        
        return Result.Success();
    }
}