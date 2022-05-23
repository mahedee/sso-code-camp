using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Commands.User
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public CreateUserCommandHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<CreateUserCommand, ApplicationUser>(request);
            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = false;
            var (identityResult, userId) = await _identityRepository.CreateUserAsync(user, request.Password);
            if (!identityResult.Succeeded)
            {
                throw new IdentityResultException(identityResult, "Create User");
            }

            var createdUser = await _identityRepository.GetUserAsync(userId);
            return _mapper.Map<ApplicationUser, UserDto>(createdUser);
        }
    }
}
