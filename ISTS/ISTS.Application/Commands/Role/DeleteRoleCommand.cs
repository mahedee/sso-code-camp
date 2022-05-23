using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.Role
{
    public class DeleteRoleCommand : IRequest<int>
    {
        public DeleteRoleCommand(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, int>
    {
        private readonly IIdentityRepository _identityRepository;

        public DeleteRoleCommandHandler(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<int> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var roleDetails = await _identityRepository.GetRoleAsync(request.Id);
            if (roleDetails == null)
            {
                throw new NotFoundException("Role information not found");
            }

            if (!roleDetails.IsDeletable)
            {
                throw new Exception("This role is not allowed to delete");
            }

            var result = await _identityRepository.DeleteRoleAsync(roleDetails);
            return !result.Succeeded ? throw new IdentityResultException(result, "Delete Role") : 1;
        }
    }
}
