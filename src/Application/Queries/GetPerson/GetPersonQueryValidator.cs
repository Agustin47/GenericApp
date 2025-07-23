using Domain.State;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetPerson;

public class GetPersonQueryValidator : QueryBaseValidator<GetPersonQuery, PersonState>;