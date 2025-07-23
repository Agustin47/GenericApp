// using Domain.Events;
// using Domain.State;
// using Framework.Database;
// using Framework.EventManager;
//
// namespace Application.Projections;
//
// public class PersonProjection(IRepositoryFactory repositoryFactory) :
//     IEventProjection<PersonCreated>,
//     IEventProjection<PersonUpdated>
// {
//     public async Task HandleAsync(PersonCreated @event)
//     {
//         var repository = repositoryFactory.GetRepository<PersonState>();
//         
//         var body = @event.Body;
//         PersonState projection = new()
//         {
//             Id = body.Id,
//             Name = body.Name,
//             LastName = body.LastName,
//             Age = body.Age,
//             Identification = body.Identification,
//             Version = body.Version
//         };
//
//         await repository.CreateAsync(projection);
//     }
//
//     public async Task HandleAsync(PersonUpdated @event)
//     {
//         var repository = repositoryFactory.GetRepository<PersonState>();
//         
//         var body = @event.Body;
//         PersonState projection = new()
//         {
//             Id = body.Id,
//             Name = body.Name,
//             LastName = body.LastName,
//             Age = body.Age,
//             Identification = body.Identification,
//             Version = body.Version
//         };
//
//         await repository.UpdateAsync(projection);
//     }
// }