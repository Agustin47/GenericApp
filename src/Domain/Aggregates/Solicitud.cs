using Domain.ValueObject;
using Framework.Common.Result;
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
    public Guid PresupuestoId { get; set; }

    public void New(
        Guid solicitanteId, Guid presupuestoId, string tipoAyuda, string detalles, Guid comunaId, string comuna, decimal monto,
        string urgencia, string justificacion, string observaciones,
        string userContext, DateTime? actionTime = null)
    {
        SolicitanteId = solicitanteId;
        PresupuestoId = presupuestoId;
        TipoAyuda = tipoAyuda;
        Detalles = detalles;
        ComunaId = comunaId;
        Comuna = comuna;
        Monto = monto;
        Gestor = userContext;
        Urgencia = urgencia;
        Justificacion = justificacion;
        Observaciones = observaciones;
        Estado = SolicitudStatus.Pendiente.Name;
        
        Fecha = FechaActualizacion = DateTime.UtcNow;
        
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
    }

    public Result Approve(string userContext, DateTime? actionTime = null)
    {
        if(Estado != SolicitudStatus.Pendiente.Name)
            return Result.Failed("No se puede cancelar la solicitud, estado invalido", "DOM-0001");
        
        Estado = SolicitudStatus.Aprobada.Name;
        
        FechaActualizacion = DateTime.UtcNow;
        
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
        
        return Result.Success();
    }
    
    public Result Cancel(string userContext, DateTime? actionTime = null)
    {
        if(Estado != SolicitudStatus.Pendiente.Name)
            return Result.Failed("No se puede cancelar la solicitud, estado invalido", "DOM-0001");
        
        Estado = SolicitudStatus.Rechazada.Name;
        
        FechaActualizacion = DateTime.UtcNow;
        
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
        
        return Result.Success();
    }
}