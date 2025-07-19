using Domain;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetPerson;

public class GetPersonQueryPermissionValidator : QueryBasePermissionValidator<GetPersonQuery, Person>
{
    protected override string Permission => Roles.Permission.UserGet.Name;
}