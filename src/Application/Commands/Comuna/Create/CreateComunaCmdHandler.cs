using Domain;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Domain;

namespace Application.Commands.Comuna.Create;

public class CreateComunaCmdHandler(IDomainEntityFactory domainEntityFactory)
    : ICommandHandler<CreateComunaCmd>
{
    public async Task<Result> Handle(CreateComunaCmd command)
    {

        EntityId comunaId = new(Guid.NewGuid());
        var comunaEntity = domainEntityFactory.Create<Domain.Aggregates.Comuna>(comunaId);
        
        comunaEntity.New(command.Nombre, command.Codigo, command.Estado, command.Poblacion,
            command.UserContext.Username);
        foreach (var ofiId in command.OficinasId)
        {
            EntityId id = new(ofiId);
            var oficina = await domainEntityFactory.GetByIdAsync<Domain.Aggregates.Oficina, EntityId>(id);
            if(oficina == null) continue;
            comunaEntity.AddOficina(oficina);
        }

        await comunaEntity.SaveChanges();
        
        return Result.Success();
    }
}