using Domain;
using Framework.Common.Result;
using Framework.CQRS.Queries;
using Framework.Database;
using Framework.Specification;

namespace Application.Queries.GetPerson;

public class GetPersonQueryHandler : IQueryHandler<GetPersonQuery, Person>
{
    private readonly IRepository<Person> _userRepository;

    public GetPersonQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _userRepository = repositoryFactory.GetRepository<Person>();
    }
    
    public async Task<Result<Person?>> Handle(GetPersonQuery query)
    {
        var spec1 = Specification<Person>.Create(u => u.Name == "Juan");

        var queryRepo = QueryRepositoryBuilder<Person>.Create()
            .AddSpecs(spec1)
            .AddFilters(query.Filters)
            .WithPaging(query.Paging)
            .WithSorting(query.Sorting)
            .Build();
        
        var users = await _userRepository.Filter(queryRepo);
        var user = await _userRepository.FirstOrDefault(queryRepo);
        
        return Result.Success(new Person(null));
    }
}