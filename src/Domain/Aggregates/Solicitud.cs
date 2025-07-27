using Framework.Database;
using Framework.Domain.Repository;

namespace Domain.Aggregates;

public class Solicitud(Guid id, IRepositoryFactory repoFactory) : DomainRepositoryEntity<Solicitud>(id, repoFactory)
{
    
    public Guid SolicitanteId { get; private set; }
    public string TipoAyuda { get; private set; }
    public string Detalles { get; private set; }
    public Guid ComunaId { get; private set; }
    public string Comuna { get; private set; }
    public decimal Monto { get; private set; }
    public string Estado { get; private set; }
    public DateTime Fecha { get; private set; }
    public DateTime FechaActualizacion { get; private set; }
    public string Gestor { get; private set; }
    public string Urgencia { get; private set; }
    public string Justificacion { get; private set; }
    public string Observaciones { get; private set; }
    public string Documentos { get; private set; }
    public DateTime FechaEntrega { get; private set; }

    public void New(
        Guid solicitanteId, string tipoAyuda, string detalles, Guid comunaId, string comuna, decimal monto,
        string urgencia, string justificacion, string observaciones,
        string userContext, DateTime? actionTime = null)
    {
        SolicitanteId = solicitanteId;
        TipoAyuda = tipoAyuda;
        Detalles = detalles;
        ComunaId = comunaId;
        Comuna = comuna;
        Monto = monto;
        Gestor = userContext;
        Urgencia = urgencia;
        Justificacion = justificacion;
        Observaciones = observaciones;
        
        Fecha = FechaActualizacion = DateTime.UtcNow;
        
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
    }
}