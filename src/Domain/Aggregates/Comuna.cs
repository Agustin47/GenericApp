using Framework.Database;
using Framework.Domain.Repository;

namespace Domain.Aggregates;

public class Comuna(Guid id, IRepositoryFactory repoFactory) : DomainRepositoryEntity<Comuna>(id, repoFactory)
{
    public string Nombre { get; private set; }
    public string Codigo { get; private set; }
    public List<Oficina> Oficinas { get; private set; }
    public string Estado { get; private set; }
    public int Poblacion { get; set; }
    

    public void New(string nombre, string codigo, string estado, int poblacion,
        string userContext, DateTime? fechaActualization = null)
    {
        Nombre = nombre;
        Codigo = codigo;
        Estado = estado;
        Poblacion = poblacion;
        Oficinas = new();
        
        Version++;
        UserContext = userContext;
        LastUpdate = fechaActualization ?? DateTime.UtcNow;
    }
    
    public void AddOficina(Oficina oficina)
    {
        Oficinas.Add(oficina);
    }
}