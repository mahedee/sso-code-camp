using FluentValidation;

namespace ISTS.Application.Commands.Role
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.RoleName).NotEmpty().NotNull();
        }
    }
}
