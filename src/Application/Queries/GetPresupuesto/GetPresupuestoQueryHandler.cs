using Domain.Aggregates;
using Framework.Common.Result;
using Framework.CQRS.Queries;
using Framework.Database;

namespace Application.Queries.GetPresupuesto;

public class GetPresupuestoQueryHandler(IRepositoryFactory repositoryFactory) : IQueryHandler<GetPresupuestoQuery, List<Presupuesto>>
{
    public async Task<Result<List<Presupuesto>?>> Handle(GetPresupuestoQuery query)
    {
        var presupuestoRepo = repositoryFactory.GetRepository<Presupuesto>(); 
        var queryRepo = QueryRepositoryBuilder<Presupuesto>.Create()
            .AddFilters(query.Filters)
            .WithPaging(query.Paging)
            .WithSorting(query.Sorting)
            .Build();
        
        var presupuestoResult = await presupuestoRepo.Filter(queryRepo);
        if (presupuestoResult.IsFailed)
            return presupuestoResult;
        
        return Result.Success(presupuestoResult.Value);
    }
}