using Domain.Aggregates;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetComuna;

public class GetComunaQuery : QueryBase<List<Comuna>>;