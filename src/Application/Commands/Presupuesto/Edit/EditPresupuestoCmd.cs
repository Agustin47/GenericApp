using Framework.CQRS.Implementation;

namespace Application.Commands.Presupuesto.Edit;

public class EditPresupuestoCmd : CommandBase
{
    public Guid EntityId { get; set; }
    public decimal Planificado { get; set; }
}