using Domain;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetUser;

public class GetUserQueryPermissionValidator : QueryBasePermissionValidator<GetUserQuery, User>
{
    protected override string Permission => Roles.Permission.UserGet.Name;
}