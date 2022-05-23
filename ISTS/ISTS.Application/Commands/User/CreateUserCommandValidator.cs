using FluentValidation;

namespace ISTS.Application.Commands.User
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm Password is required")
                .Must((x, confirmPass) => x.Password.Equals(confirmPass, StringComparison.InvariantCulture))
                .WithMessage("Password and Confirm Password does not match");
        }
    }
}
