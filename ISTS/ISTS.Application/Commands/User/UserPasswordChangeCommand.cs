using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.User
{
    public class UserPasswordChangeCommand : IRequest<int>
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class UserPasswordChangeCommandHandler : IRequestHandler<UserPasswordChangeCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public UserPasswordChangeCommandHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<int> Handle(UserPasswordChangeCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityRepository.GetUserAsync(request.UserId);
            if (userDetails == null)
                throw new NotFoundException("User Information not found");
            var result = await _identityRepository.UserChangePasswordAsync(request.UserId, request.Password);
            if (!result.Succeeded)
            {
                throw new IdentityResultException(result, "Change Password");
            }

            return 1;
        }
    }
}
