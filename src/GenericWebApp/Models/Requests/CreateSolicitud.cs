namespace GenericWebApp.Models.Requests;

public record CreateSolicitud(Guid SolicitanteId, string TipoAyuda, string Detalles, Guid ComunaId, string Comuna, decimal Monto, string Estado, string Gestor, string Urgencia, string Justificacion, string Observaciones);
