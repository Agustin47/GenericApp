using Domain;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetUser;

public class GetUserQueryValidator : QueryBaseValidator<GetUserQuery, User>
{
    
}