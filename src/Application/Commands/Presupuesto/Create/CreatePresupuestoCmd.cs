using Framework.CQRS.Implementation;

namespace Application.Commands.Presupuesto.Create;

public class CreatePresupuestoCmd : CommandBase
{
    public string Comuna { get; set; }
    public Guid ComunaId { get; set; }
    public decimal Planificado { get; set; }
    public int Mes { get; set; }
}