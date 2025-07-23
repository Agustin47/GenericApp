using Framework.Common.Result;
using Framework.CQRS.Queries;
using Framework.Database;
using Framework.Specification;
using Domain.State;

namespace Application.Queries.GetPerson;

public class GetPersonQueryHandler : IQueryHandler<GetPersonQuery, PersonState>
{
    private readonly IRepository<PersonState> _userRepository;

    public GetPersonQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _userRepository = repositoryFactory.GetRepository<PersonState>();
    }
    
    public async Task<Result<PersonState?>> Handle(GetPersonQuery query)
    {
        var spec1 = Specification<PersonState>.Create(u => u.Nombre == "juan");

        var queryRepo = QueryRepositoryBuilder<PersonState>.Create()
            .AddSpecs(spec1)
            .AddFilters(query.Filters)
            .WithPaging(query.Paging)
            .WithSorting(query.Sorting)
            .Build();
        
        var users = await _userRepository.Filter(queryRepo);
        var user = await _userRepository.FirstOrDefault(queryRepo);
        
        return user;
    }
}