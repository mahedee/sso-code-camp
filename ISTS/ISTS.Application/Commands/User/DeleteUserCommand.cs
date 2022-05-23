using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.User
{
    public class DeleteUserCommand : IRequest<int>
    {
        public DeleteUserCommand(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IIdentityRepository _identityRepository;

        public DeleteUserCommandHandler(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityRepository.GetUserAsync(request.Id);
            if (userDetails == null)
            {
                throw new NotFoundException("Invalid User Information");
            }

            if (!userDetails.IsDeletable)
            {
                throw new Exception("This user is not allowed to delete");
            }

            var result = await _identityRepository.DeleteUserAsync(request.Id);
            if (!result.Succeeded)
            {
                throw new IdentityResultException(result, "Delete User");
            }

            return 1;
        }
    }
}
