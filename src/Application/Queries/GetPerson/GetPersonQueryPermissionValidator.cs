using Domain.Aggregates;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetPerson;

public class GetPersonQueryPermissionValidator : QueryBasePermissionValidator<GetPersonQuery, List<Persona>>
{
    protected override string Permission => Roles.Permission.UserGet.Name;
}