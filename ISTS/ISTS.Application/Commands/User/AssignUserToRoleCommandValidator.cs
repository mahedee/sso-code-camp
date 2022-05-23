using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ISTS.Application.Common.Interfaces.Identity;

namespace ISTS.Application.Commands.User
{
    public class AssignUserToRoleCommandValidator : AbstractValidator<AssignUserToRoleCommand>
    {
        private readonly IIdentityRepository _identityRepository;
        public AssignUserToRoleCommandValidator(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
            RuleForEach(x => x.RoleId)
                .NotEmpty().WithMessage("Role ID is required")
                .MustAsync(async (role, token) =>
                {
                    var roleExists = await _identityRepository.ExistsRoleAsync(role);
                    return roleExists;
                }).WithMessage("Invalid role information");
            RuleForEach(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required")
                .MustAsync(async (userId, token) =>
            {
                var userDetails = await _identityRepository.ExistsUserAsync(userId);
                return userDetails;
            }).WithMessage("Invalid User information");
        }
    }
}
