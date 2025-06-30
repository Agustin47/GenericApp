using Framework.CQRS.Implementation;

namespace Application.Commands.Weather;

public class WeatherCommandPermissionValidator : CommandBasePermissionValidator<WeatherCommand>
{
    protected override string Permission => "Weather";
}