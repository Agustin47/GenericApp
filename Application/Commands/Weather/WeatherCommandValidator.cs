using Framework.CQRS.Commands;

namespace Application.Commands.Weather;

public class WeatherCommandValidator : ICommandValidator<WeatherCommand>
{
    public WeatherCommandValidator()
    {
        // fluent validator
        //RuleFor
    }
    

    public bool Validate(WeatherCommand command)
    {
        throw new NotImplementedException();
    }
}