using Domain.State;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetPerson;

public class GetPersonQueryPermissionValidator : QueryBasePermissionValidator<GetPersonQuery, PersonState>
{
    protected override string Permission => Roles.Permission.UserGet.Name;
}