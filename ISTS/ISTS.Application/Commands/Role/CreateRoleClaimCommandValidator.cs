using FluentValidation;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;

namespace ISTS.Application.Commands.Role
{
    public class CreateRoleClaimCommandValidator : AbstractValidator<CreateRoleClaimCommand>
    {
        public CreateRoleClaimCommandValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().NotNull();
            RuleFor(x => x.ClaimType).NotEmpty().NotNull();
            RuleFor(x => x.ClaimValue).NotEmpty().NotNull();
        }
    }
}
