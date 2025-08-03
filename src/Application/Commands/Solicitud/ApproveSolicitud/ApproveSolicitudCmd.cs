using Framework.CQRS.Implementation;

namespace Application.Commands.Solicitud.ApproveSolicitud;

public class ApproveSolicitudCmd : CommandBase
{
    public Guid EntityId { get; set; }
}