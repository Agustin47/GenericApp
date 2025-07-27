using Domain.Aggregates;
using Framework.Common.Result;
using Framework.CQRS.Queries;
using Framework.Database;
using Framework.Specification;

namespace Application.Queries.GetPerson;

public class GetPersonQueryHandler : IQueryHandler<GetPersonQuery, List<Persona>>
{
    private readonly IRepository<Persona> _userRepository;

    public GetPersonQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _userRepository = repositoryFactory.GetRepository<Persona>();
    }
    
    public async Task<Result<List<Persona>>> Handle(GetPersonQuery query)
    {
        var queryRepo = QueryRepositoryBuilder<Persona>.Create()
            .AddFilters(query.Filters)
            .WithPaging(query.Paging)
            .WithSorting(query.Sorting)
            .Build();
        
        var users = await _userRepository.Filter(queryRepo);
        
        return users;
    }
}