using Domain.State;
using Framework.CQRS.Implementation;

namespace Application.Queries.GetPerson;

public class GetPersonQuery : QueryBase<List<PersonState>>;