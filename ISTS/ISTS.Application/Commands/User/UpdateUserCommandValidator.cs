using FluentValidation;

namespace ISTS.Application.Commands.User
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            //RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
        }
    }
}
