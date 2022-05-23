using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Commands.User
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public UpdateUserCommandHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userDetails = await _identityRepository.GetUserAsync(request.Id);

            if (userDetails is null)
            {
                throw new NotFoundException("Invalid User Information");
            }

            var mappedUser = _mapper.Map<UpdateUserCommand, ApplicationUser>(request, userDetails);

            var (identityResult, userId) = await _identityRepository.UpdateUserAsync(mappedUser);
            if (!identityResult.Succeeded)
            {
                throw new IdentityResultException(identityResult, "Update User");
            }

            var userInfo = await _identityRepository.GetUserAsync(userId);

            return _mapper.Map<ApplicationUser, UserDto>(userInfo);
        }
    }
}
