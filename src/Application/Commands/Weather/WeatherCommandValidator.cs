using Framework.CQRS.Implementation;

namespace Application.Commands.Weather;

public class WeatherCommandValidator : CommandBaseValidator<WeatherCommand>
{
    public WeatherCommandValidator()
    {
        
    }
}