namespace Domain.Aggregates;

public class Proposal
{
    
    public string solicitante { get; private set; }
    public string tiposAyuda { get; private set; }
    public string detalles { get; private set; }
    public string comuna { get; private set; }
    public string monto { get; private set; }
    public string estado { get; private set; }
    public string fecha { get; private set; }
    public string fechaActualizacion { get; private set; }
    public object gestor { get; private set; }
    public string urgencia { get; private set; }
    public string justificacion { get; private set; }
    public string observaciones { get; private set; }
    public string documentos { get; private set; }
    public string fechaEntrega { get; private set; }
}