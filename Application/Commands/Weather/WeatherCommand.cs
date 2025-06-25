using Framework.CQRS.Commands;

namespace Application.Commands.Weather;

public class WeatherCommand : ICommand
{
    public string Value { get; set; }
}