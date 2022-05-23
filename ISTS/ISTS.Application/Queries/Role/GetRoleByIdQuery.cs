using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Queries.Role
{
    public class GetRoleByIdQuery : IRequest<RoleDto>
    {
        public string Id { get; set; }
    }

    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;

        public GetRoleByIdQueryHandler(IMapper mapper, IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
        }

        public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var roleInformation = await _identityRepository.GetRoleAsync(request.Id);
            return _mapper.Map<ApplicationRole, RoleDto>(roleInformation);
        }
    }
}
