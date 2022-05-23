using FluentValidation;

namespace ISTS.Application.Commands.User
{
    public class UserPasswordChangeCommandValidator : AbstractValidator<UserPasswordChangeCommand>
    {
        public UserPasswordChangeCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotNull().NotEmpty()
                .Equal(x => x.Password).WithMessage("Password and Confirm Password does not match");
        }
    }
}
