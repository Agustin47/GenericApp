using FluentValidation;
using Framework.CQRS.Implementation;

namespace Application.Commands.UserCreation;

public class CreateUserCmdValidation : CommandBaseValidator<CreateUserCmd>
{
    public CreateUserCmdValidation()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("UserName is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}