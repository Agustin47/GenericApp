using Domain.Aggregates;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetOficina;

public class GetOficinaQuery : QueryBase<List<Oficina>>;