using Framework.Common.Result;
using Framework.Database;
using Framework.Domain.Repository;

namespace Domain.Aggregates;

public class Presupuesto(Guid id, IRepositoryFactory repoFactory) : DomainRepositoryEntity<Presupuesto>(id, repoFactory)
{
    public string Comuna { get; private set; }
    public Guid ComunaId { get; private set; }
    public decimal Planificado { get; private set; }
    public decimal Gastado { get; private set; }
    public int Mes { get; private set; }
    public int Año { get; private set; }
    public DateTime FechaAplica { get; private set; }
    public int NumeroSolicitudes { get; private set; }
    public int NumeroPersonas { get; private set; }
    public List<SolicitudResumen> Solicitudes { get; private set; }
    
    
    public void New(string comuna, Guid comunaId, decimal planificado, int mes, int año,
        string userContext, DateTime? actionTime = null)
    {
        Comuna = comuna;
        ComunaId = comunaId;
        Planificado = planificado;
        Mes = mes;
        Año = año;
        FechaAplica = new(año, mes, 1);
        Gastado = 0;
        NumeroSolicitudes = 0;
        NumeroPersonas = 0;
        Solicitudes = new();
        
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
    }
    
    public void AddGastado(SolicitudResumen solicitud,
        string userContext, DateTime? actionTime = null)
    {
        Gastado += solicitud.Monto;
        NumeroSolicitudes++;
        var exitsSolicitante = Solicitudes.FirstOrDefault(x => x.SolicitanteId == solicitud.SolicitanteId);
        if (exitsSolicitante == null)
            NumeroPersonas++;
        Solicitudes.Add(solicitud);
        
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
    }

    public void RemoveGastado(Guid solicitudId,
        string userContext, DateTime? actionTime = null)
    {
        var solicitud = Solicitudes.FirstOrDefault(x => x.Id == solicitudId);
        if (solicitud == null)
            return;
        
        Gastado -= solicitud.Monto;
        NumeroSolicitudes--;
        Solicitudes.Remove(solicitud);
        
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
    }

    public Result EditPresupuesto(decimal newPlanificado,
        string userContext, DateTime? actionTime = null)
    {
        if(newPlanificado < Gastado)
            return Result.Failed("El nuevo planificado debe ser mayor al gastado", "DOM-0001");
        
        Planificado = newPlanificado;
        
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
        
        return Result.Success();
    }

    public class SolicitudResumen
    {
        public required Guid Id { get; set; }
        public required Guid SolicitanteId { get; set; }
        public required string SolicitanteNombre { get; set; }
        public required string SolicitanteDNI { get; set; }
        public required decimal Monto { get; set; }
    }
}