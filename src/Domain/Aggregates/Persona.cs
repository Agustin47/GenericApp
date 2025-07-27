using Domain.State;
using Framework.Database;
using Framework.Domain.Repository;

namespace Domain.Aggregates;

public class Persona : DomainRepositoryEntity<Persona>
{
    public Persona(Guid id, IRepositoryFactory repoFactory) : base(id, repoFactory) { }

    public string Dni { get; private set; }
    public string Nombre { get; private set; }
    public string Apellido { get; private set; }
    public string Telefono { get; private set; }
    public string Email { get; private set; }
    public string Direccion { get; private set; }
    public string Comuna { get; private set; }
    public string Barrio { get; private set; }
    public string FechaNacimiento { get; private set; }
    public string AnioNacimiento { get; private set; }
    public string TipoDocumento { get; private set; }
    public string Genero { get; private set; }
    public string EstadoCivil { get; private set; }
    public string Ocupacion { get; private set; }
    public string IngresosFamiliares { get; private set; }
    public string NumeroFamiliares { get; private set; }
    public string TipoVivienda { get; private set; }
    public string ServiciosBasicos { get; private set; }
    public string DechaRegistro { get; private set; }
    public string UltimaActualizacion { get; private set; }
    public string Estado { get; private set; }
    public string TotalSolicitudes { get; private set; }
    public string MontoTotalRecibido { get; private set; }
    public string UltimaSolicitud { get; private set; }
    public string Observaciones { get; private set; }
    public string Obs { get; private set; }
    
    
    public async Task Register(string dni, string nombre, string apellido, string telefono, string email, string direccion, string comuna, string barrio, string fechaNacimiento, 
        string anioNacimiento, string tipoDocumento, string genero, string estadoCivil, string ocupacion, string ingresosFamiliares, string numeroFamiliares, string tipoVivienda, 
        string serviciosBasicos, string dechaRegistro, string ultimaActualizacion, string estado, string totalSolicitudes, string montoTotalRecibido, string ultimaSolicitud, 
        string observaciones, string obs,
        string userContext, DateTime? actionTime = null)
    {
        Dni = dni;
        Nombre = nombre; 
        Apellido = apellido;
        Telefono = telefono;
        Email = email;
        Direccion = direccion;
        Comuna = comuna;
        Barrio = barrio;
        FechaNacimiento = fechaNacimiento;
        AnioNacimiento = anioNacimiento;
        TipoDocumento = tipoDocumento;
        Genero = genero;
        EstadoCivil = estadoCivil;
        Ocupacion = ocupacion;
        IngresosFamiliares = ingresosFamiliares;
        NumeroFamiliares = numeroFamiliares;
        TipoVivienda = tipoVivienda;
        ServiciosBasicos = serviciosBasicos;
        DechaRegistro = dechaRegistro;
        UltimaActualizacion = ultimaActualizacion;
        Estado = estado;
        TotalSolicitudes = totalSolicitudes;
        MontoTotalRecibido = montoTotalRecibido;
        UltimaSolicitud = ultimaSolicitud;
        Observaciones = observaciones;
        Obs = obs;

        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
        // await PushEvent(new PersonCreated
        // {
        //     Body = ToState(),
        //     Username = userContext,
        //     CreatedAt = actionTime ?? DateTime.UtcNow,
        // });
    }
    
    public async Task Update(string dni, string nombre, string apellido, string telefono, string email, string direccion, string comuna, string barrio, string fechaNacimiento, 
         string anioNacimiento, string tipoDocumento, string genero, string estadoCivil, string ocupacion, string ingresosFamiliares, string numeroFamiliares, string tipoVivienda, 
         string serviciosBasicos, string dechaRegistro, string ultimaActualizacion, string estado, string totalSolicitudes, string montoTotalRecibido, string ultimaSolicitud, 
         string observaciones, string obs,
            string userContext, DateTime? actionTime = null)
    {
        Dni = dni;
        Nombre = nombre; 
        Apellido = apellido;
        Telefono = telefono;
        Email = email;
        Direccion = direccion;
        Comuna = comuna;
        Barrio = barrio;
        FechaNacimiento = fechaNacimiento;
        AnioNacimiento = anioNacimiento;
        TipoDocumento = tipoDocumento;
        Genero = genero;
        EstadoCivil = estadoCivil;
        Ocupacion = ocupacion;
        IngresosFamiliares = ingresosFamiliares;
        NumeroFamiliares = numeroFamiliares;
        TipoVivienda = tipoVivienda;
        ServiciosBasicos = serviciosBasicos;
        DechaRegistro = dechaRegistro;
        UltimaActualizacion = ultimaActualizacion;
        Estado = estado;
        TotalSolicitudes = totalSolicitudes;
        MontoTotalRecibido = montoTotalRecibido;
        UltimaSolicitud = ultimaSolicitud;
        Observaciones = observaciones;
        Obs = obs;

        Version++;
        UserContext = userContext;
        LastUpdate = actionTime ?? DateTime.UtcNow;
        // await PushEvent(new PersonUpdated
        // {
        //     Body = ToState(),
        //     Username = userContext,
        //     CreatedAt = actionTime ?? DateTime.UtcNow,
        // });
    }

    // todo comentado, eventos.
    
    // public override void Apply(IEvent @event)
    // {
    //     switch (@event)
    //     {
    //         case PersonCreated personCreated:
    //             var state = personCreated.Body;
    //             Dni = state.Dni;
    //             Nombre = state.Nombre;
    //             Apellido = state.Apellido;
    //             Telefono = state.Telefono;
    //             Email = state.Email;
    //             Direccion = state.Direccion;
    //             Comuna = state.Comuna;
    //             Barrio = state.Barrio;
    //             FechaNacimiento = state.FechaNacimiento;
    //             AnioNacimiento = state.AnioNacimiento;
    //             TipoDocumento = state.TipoDocumento;
    //             Genero = state.Genero;
    //             EstadoCivil = state.EstadoCivil;
    //             Ocupacion = state.Ocupacion;
    //             IngresosFamiliares = state.IngresosFamiliares;
    //             NumeroFamiliares = state.NumeroFamiliares;
    //             TipoVivienda = state.TipoVivienda;
    //             ServiciosBasicos = state.ServiciosBasicos;
    //             DechaRegistro = state.DechaRegistro;
    //             UltimaActualizacion = state.UltimaActualizacion;
    //             Estado = state.Estado;
    //             TotalSolicitudes = state.TotalSolicitudes;
    //             MontoTotalRecibido = state.MontoTotalRecibido;
    //             UltimaSolicitud = state.UltimaSolicitud;
    //             Observaciones = state.Observaciones;
    //             Obs = state.Obs;
    //             break;
    //         case PersonUpdated personUpdated:
    //             state = personUpdated.Body;
    //             Dni = state.Dni;
    //             Nombre = state.Nombre;
    //             Apellido = state.Apellido;
    //             Telefono = state.Telefono;
    //             Email = state.Email;
    //             Direccion = state.Direccion;
    //             Comuna = state.Comuna;
    //             Barrio = state.Barrio;
    //             FechaNacimiento = state.FechaNacimiento;
    //             AnioNacimiento = state.AnioNacimiento;
    //             TipoDocumento = state.TipoDocumento;
    //             Genero = state.Genero;
    //             EstadoCivil = state.EstadoCivil;
    //             Ocupacion = state.Ocupacion;
    //             IngresosFamiliares = state.IngresosFamiliares;
    //             NumeroFamiliares = state.NumeroFamiliares;
    //             TipoVivienda = state.TipoVivienda;
    //             ServiciosBasicos = state.ServiciosBasicos;
    //             DechaRegistro = state.DechaRegistro;
    //             UltimaActualizacion = state.UltimaActualizacion;
    //             Estado = state.Estado;
    //             TotalSolicitudes = state.TotalSolicitudes;
    //             MontoTotalRecibido = state.MontoTotalRecibido;
    //             UltimaSolicitud = state.UltimaSolicitud;
    //             Observaciones = state.Observaciones;
    //             Obs = state.Obs;
    //             break;
    //     }
    // }
}

/*
   public string Idnumber { get; set; }
   public string Telefono { get; set; }
   public string Email { get; set; }
   public string Address { get; set; }
   public string TownId { get; set; }
   public string TownName { get; set; }
   public string Neighborhood { get; set; }
   public string BirdDate { get; set; }
   public string BirdYear { get; set; }
   public string IdType { get; set; }
   public string Gender { get; set; }
   public string MaritalStatus { get; set; }
   public string Position { get; set; }
   public string FamilyIngress { get; set; }
   public string FamilyNumber { get; set; }
   public string HomeType { get; set; }
   public string BasicServices { get; set; }
   public string RegisterDate { get; set; }
   public string LastUpdate { get; set; }
   public string State { get; set; }
   public string ProposalsAmount { get; set; }
   public string TotalReceived { get; set; }
   public string LastProposal { get; set; }
   public string Remarks { get; set; }
   public string Remarks2 { get; set; }
   
*/