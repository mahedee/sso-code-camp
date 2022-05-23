using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.User
{
    public class DeleteUserRoleCommand : IRequest<int>
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, int>
    {
        private readonly IIdentityRepository _identityRepository;

        public DeleteUserRoleCommandHandler(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<int> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityRepository.GetUserAsync(request.UserId);
            if (userDetails == null)
                throw new NotFoundException("User information not found");

            var roleDetails = await _identityRepository.GetRoleAsync(request.RoleId);
            if (roleDetails == null)
                throw new NotFoundException("No role information found");

            var result = await _identityRepository.DeleteUserRoleAsync(request.UserId, request.RoleId);
            if (!result.Succeeded)
            {
                throw new IdentityResultException(result, "Delete User Role");
            }

            return 1;
        }
    }
}
