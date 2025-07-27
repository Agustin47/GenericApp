using Framework.Database;
using Framework.Domain.Repository;

namespace Domain.Aggregates;

public class Oficina(Guid id, IRepositoryFactory repoFactory) : DomainRepositoryEntity<Oficina>(id, repoFactory)
{
    public string Nombre { get; private set; }
    public string Codigo { get; private set; }
    public Guid ComunaId { get; private set; }
    public string Comuna { get; private set; }
    public string Direccion { get; private set; }
    public string Telefono { get; private set; }
    public string Estado { get; private set; }
 
    
    public void New(string nombre, string codigo, Guid comunaId, string comuna, string direccion, string telefono, string estado,
        string userContext, DateTime? actionTime = null)
    {
        Nombre = nombre;
        Codigo = codigo;
        ComunaId = comunaId;
        Comuna = comuna;
        Direccion = direccion;
        Telefono = telefono;
        Estado = estado;
        
        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
    }
}