using Domain;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Domain;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Person.Create;

public class CreatePersonCmdHandler(IDomainEntityFactory domainEntityFactory) : ICommandHandler<CreatePersonCmd>
{
    public async Task<Result> Handle(CreatePersonCmd command)
    {
        Guid id = Guid.NewGuid();
        EntityId entityId = new(id);
        var person = domainEntityFactory.Create<Domain.Aggregates.Persona>(entityId);

        await person.Register(command.Dni, command.Nombre, command.Apellido, command.Telefono, command.Email,
            command.Direccion, command.Comuna, command.Barrio, command.FechaNacimiento, command.AnioNacimiento,
            command.TipoDocumento, command.Genero, command.EstadoCivil, command.Ocupacion, command.IngresosFamiliares,
            command.NumeroFamiliares, command.TipoVivienda, command.ServiciosBasicos, command.DechaRegistro,
            command.UltimaActualizacion, command.Estado, command.TotalSolicitudes, command.MontoTotalRecibido,
            command.UltimaSolicitud, command.Observaciones, command.Obs,
            command.UserContext.Username);
        
        
        await person.SaveChanges();
        return Result.Success();
    }
}