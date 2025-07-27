using Domain.Aggregates;
using Framework.Common.Result;
using Framework.CQRS.Queries;
using Framework.Database;

namespace Application.Queries.GetComuna;

public class GetComunaQueryHandler(IRepositoryFactory repositoryFactory) : IQueryHandler<GetComunaQuery, List<Comuna>>
{
    public async Task<Result<List<Comuna>?>> Handle(GetComunaQuery query)
    {
        var comunaRepo = repositoryFactory.GetRepository<Comuna>(); 
        var queryRepo = QueryRepositoryBuilder<Comuna>.Create()
            .AddFilters(query.Filters)
            .WithPaging(query.Paging)
            .WithSorting(query.Sorting)
            .Build();
        
        var comunaResult = await comunaRepo.Filter(queryRepo);
        if (comunaResult.IsFailed)
            return comunaResult;
        
        return Result.Success(comunaResult.Value);
    }
}