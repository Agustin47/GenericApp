using Domain.Aggregates;
using Framework.Common.Result;
using Framework.CQRS.Queries;
using Framework.Database;

namespace Application.Queries.GetOficina;

public class GetOficinaQueryHandler(IRepositoryFactory repositoryFactory) : IQueryHandler<GetOficinaQuery, List<Oficina>>
{
    public async Task<Result<List<Oficina>?>> Handle(GetOficinaQuery query)
    {
        var oficinaRepo = repositoryFactory.GetRepository<Oficina>(); 
        var queryRepo = QueryRepositoryBuilder<Oficina>.Create()
            .AddFilters(query.Filters)
            .WithPaging(query.Paging)
            .WithSorting(query.Sorting)
            .Build();
        
        var oficinaResult = await oficinaRepo.Filter(queryRepo);
        if (oficinaResult.IsFailed)
            return oficinaResult;
        
        return Result.Success(oficinaResult.Value);
    }
}