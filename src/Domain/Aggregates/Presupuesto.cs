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
    public int NumeroSolicitudes { get; private set; }
    
    
    public void New(string comuna, Guid comunaId, decimal planificado, int mes,
        string userContext, DateTime? actionTime = null)
    {
        Comuna = comuna;
        ComunaId = comunaId;
        Planificado = planificado;
        Mes = mes;
        Gastado = 0;
        NumeroSolicitudes = 0;
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
    }
    
    public void AddGastado(decimal gastado,
        string userContext, DateTime? actionTime = null)
    {
        Gastado += gastado;
        
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
    }
}