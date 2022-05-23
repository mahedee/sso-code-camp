using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.IdentityResource
{
    public class DeleteIdentityResourceCommand : IRequest<int>
    {
        public DeleteIdentityResourceCommand(int identityResourceId)
        {
            IdentityResourceId = identityResourceId;
        }

        public int IdentityResourceId { get; private set; }
    }

    public class DeleteIdentityResourceCommandHandler : IRequestHandler<DeleteIdentityResourceCommand, int>
    {
        private readonly IIdentityResourceRepository _identityResourceRepository;

        public DeleteIdentityResourceCommandHandler(IIdentityResourceRepository identityResourceRepository)
        {
            _identityResourceRepository = identityResourceRepository;
        }

        public async Task<int> Handle(DeleteIdentityResourceCommand request, CancellationToken cancellationToken)
        {
            var identityResourceInfo =
                await _identityResourceRepository.GetIdentityResourceAsync(request.IdentityResourceId);
            if (identityResourceInfo == null)
            {
                throw new NotFoundException("Identity resource information not found");
            }

            var result = await _identityResourceRepository.DeleteIdentityResourceAsync(identityResourceInfo);
            return result;
        }
    }
}
