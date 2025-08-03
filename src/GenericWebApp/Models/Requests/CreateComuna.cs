namespace GenericWebApp.Models.Requests;

public class CreateComuna
{
    public string Nombre { get; set; }
    public string Codigo { get; set; }
    public List<Guid> OficinasId { get; set; }
    public string Estado { get; set; }
    public int Poblacion { get; set; }
}