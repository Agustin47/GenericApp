using Framework.CQRS.Implementation;

namespace Application.Commands.Solicitud.CreateSolicitud;

public class CreateSolicitudCmd : CommandBase
{
    public Guid SolicitanteId { get; set; }
    public string TipoAyuda { get; set; }
    public string Detalles { get; set; }
    public Guid ComunaId { get; set; }
    public string Comuna { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; }
    public string Gestor { get; set; }
    public string Urgencia { get; set; }
    public string Justificacion { get; set; }
    public string Observaciones { get; set; }
}