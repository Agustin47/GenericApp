using Domain;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Domain;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Person.Update;

public class UpdatePersonCmdHandler(IDomainEntityFactory domainEntityFactory, ILogger<UpdatePersonCmdHandler> logger) : ICommandHandler<UpdatePersonCmd>
{
    public async Task<Result> Handle(UpdatePersonCmd command)
    {
        PersonId personId = new(command.Id);

        var person = await domainEntityFactory.GetByIdAsync<Domain.Aggregates.Person>(personId);
        await person.Update(command.Dni, command.Nombre, command.Apellido, command.Telefono, command.Email,
            command.Direccion, command.Comuna, command.Barrio, command.FechaNacimiento, command.AnioNacimiento,
            command.TipoDocumento, command.Genero, command.EstadoCivil, command.Ocupacion, command.IngresosFamiliares,
            command.NumeroFamiliares, command.TipoVivienda, command.ServiciosBasicos, command.DechaRegistro,
            command.UltimaActualizacion, command.Estado, command.TotalSolicitudes, command.MontoTotalRecibido,
            command.UltimaSolicitud, command.Observaciones, command.Obs,
            command.UserContext.Username);
        
        return Result.Success();
    }
}