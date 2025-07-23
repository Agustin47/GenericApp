using Framework.CQRS.Implementation;

namespace Application.Commands.Person.Create;

public class CreatePersonCmd : CommandBase
{
    public string Dni { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Direccion { get; set; }
    public string Comuna { get; set; }
    public string Barrio { get; set; }
    public string FechaNacimiento { get; set; }
    public string AnioNacimiento { get; set; }
    public string TipoDocumento { get; set; }
    public string Genero { get; set; }
    public string EstadoCivil { get; set; }
    public string Ocupacion { get; set; }
    public string IngresosFamiliares { get; set; }
    public string NumeroFamiliares { get; set; }
    public string TipoVivienda { get; set; }
    public string ServiciosBasicos { get; set; }
    public string DechaRegistro { get; set; }
    public string UltimaActualizacion { get; set; }
    public string Estado { get; set; }
    public string TotalSolicitudes { get; set; }
    public string MontoTotalRecibido { get; set; }
    public string UltimaSolicitud { get; set; }
    public string Observaciones { get; set; }
    public string Obs { get; set; }
}