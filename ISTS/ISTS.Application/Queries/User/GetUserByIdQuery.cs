using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Queries.User
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public string UserId { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public GetUserByIdQueryHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userInfo = await _identityRepository.GetUserAsync(request.UserId);
            if (userInfo is null)
            {
                throw new NotFoundException("Invalid User Information");
            }
            return _mapper.Map<ApplicationUser, UserDto>(userInfo);
        }
    }
}
