using Domain.Aggregates;
using Framework.Common.Result;
using Framework.CQRS.Queries;
using Framework.Database;

namespace Application.Queries.GetSolicitud;

public class GetSolicitudQueryHandler(IRepositoryFactory repositoryFactory) : IQueryHandler<GetSolicitudQuery, List<Solicitud>> 
{
    public async Task<Result<List<Solicitud>?>> Handle(GetSolicitudQuery query)
    {
        var solicitudRepo = repositoryFactory.GetRepository<Solicitud>(); 
        var queryRepo = QueryRepositoryBuilder<Solicitud>.Create()
            .AddFilters(query.Filters)
            .WithPaging(query.Paging)
            .WithSorting(query.Sorting)
            .Build();
        
        var solicitudResult = await solicitudRepo.Filter(queryRepo);
        if (solicitudResult.IsFailed)
            return solicitudResult;
        
        return Result.Success(solicitudResult.Value);
    }
}