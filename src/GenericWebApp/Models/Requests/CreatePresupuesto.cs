namespace GenericWebApp.Models.Requests;

public record CreatePresupuesto(string Comuna, Guid ComunaId, decimal Planificado, int Mes, int Año);