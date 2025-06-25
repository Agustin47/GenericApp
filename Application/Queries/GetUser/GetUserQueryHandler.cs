using Domain;
using Framework.Common.Result;
using Framework.CQRS.Queries;
using Framework.Database;
using Framework.Specification;

namespace Application.Queries.GetUser;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, User>
{
    private readonly IRepository<User> _userRepository;

    public GetUserQueryHandler(IRepositoryFactory repositoryFactory)
    {
        _userRepository = repositoryFactory.GetRepository<User>();
    }
    
    public async Task<Result<User?>> Handle(GetUserQuery query)
    {
        
        var spec1 = Specification<User>.Create(u => u.Name == "Juan");
        var users = await _userRepository.GetAsync(u => u.Name == "Juan");

        var user = users.Value.FirstOrDefault();
        return Result.Success(user);
    }
}