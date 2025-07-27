using Domain.Aggregates;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetPresupuesto;

public class GetPresupuestoQuery : QueryBase<List<Presupuesto>>;