using Framework.CQRS.Implementation;

namespace Application.Commands.Weather;

public class WeatherCommand : CommandBase
{
    public string Value { get; set; }
}