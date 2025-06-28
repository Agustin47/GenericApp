using Domain;
using Framework.Common.Result;
using Framework.CQRS.Commands;
using Framework.Database;

namespace Application.Commands.Weather;

public class WeatherCommandHandler : ICommandHandler<WeatherCommand>
{
    private readonly IRepository<User> _userRepository;

    public WeatherCommandHandler(IRepositoryFactory repositoryFactory)
    {
        _userRepository = repositoryFactory.GetRepository<User>();
    }
    
    public async Task<Result> Handle(WeatherCommand command)
    {
        UserId id = new(Guid.NewGuid());
        User newUser = new(id);
        newUser.AsingName("Juan", "yo");
        
        var json = System.Text.Json.JsonSerializer.Serialize(newUser);

        return await _userRepository.CreateAsync(newUser);
    }
}

public class Persona
{
    public string Name { get; set; }
    public int Age { get; set; }
}