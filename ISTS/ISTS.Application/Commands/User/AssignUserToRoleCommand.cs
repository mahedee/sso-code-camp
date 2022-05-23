using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.User
{
    public class AssignUserToRoleCommand : IRequest<int>
    {
        public List<string> UserId { get; set; }
        public List<string> RoleId { get; set; }
    }

    public class AssignUserToRoleCommandHandler : IRequestHandler<AssignUserToRoleCommand, int>
    {
        private readonly IIdentityRepository _identityRepository;

        public AssignUserToRoleCommandHandler(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<int> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
        {
            var counter = 0;
            foreach (var roleId in request.RoleId)
            {
                foreach (var userId in request.UserId)
                {
                    var userDetails = await _identityRepository.GetUserAsync(userId);
                    if (userDetails == null)
                        throw new NotFoundException("User information not found");

                    var roleDetails = await _identityRepository.GetRoleAsync(roleId);
                    if (roleDetails == null)
                        throw new NotFoundException("No role information found");
                    if (await _identityRepository.IsUserInRoleAsync(roleDetails.Name, userId)) continue;
                    var result = await _identityRepository.CreateUserRoleAsync(userId, roleId);
                    if (!result.Succeeded)
                    {
                        throw new IdentityResultException(result, "Assign role to User");
                    }

                    counter++;
                }
            }

            return counter;
        }
    }
}
