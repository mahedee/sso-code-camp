using FluentValidation;

namespace ISTS.Application.Commands.Role
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Role Name is required");
        }
    }
}
