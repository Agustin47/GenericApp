using Framework.Common;

namespace Domain.ValueObject;

public class SolicitudStatus
{
    public static SolicitudStatus Pendiente = new(1, "Pendiente");
    public static SolicitudStatus Aprobada = new(2, "Aprobada");
    public static SolicitudStatus Rechazada = new(3, "Rechazada");
    public static SolicitudStatus Entregada = new(4, "Entregada");
    
    
    public int Id { get; private set; }
    public string Name { get; private set; }
    
    private SolicitudStatus(int id, string name){ Id = id; Name = name;}
    
    public static SolicitudStatus? GetByName(string name) =>
        ValueObjectExtensions.GetAllOptionsAsList<SolicitudStatus>().FirstOrDefault(x => x.Name == name);
}