using Domain.Aggregates;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetPerson;

public class GetPersonQueryValidator : QueryBaseValidator<GetPersonQuery, List<Person>>;