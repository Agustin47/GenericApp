using Framework.CQRS.Implementation;

namespace Application.Commands.Solicitud.CancelSolicitud;

public class CancelSolicitudCmd : CommandBase
{
    public Guid EntityId { get; set; }
}