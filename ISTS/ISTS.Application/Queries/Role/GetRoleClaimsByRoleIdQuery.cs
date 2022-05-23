using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities.Identity;
using MediatR;

namespace ISTS.Application.Queries.Role
{
    public class GetRoleClaimsByRoleIdQuery : IRequest<IEnumerable<RoleClaimDto>>
    {
        public string RoleId { get; set; }
    }

    public class GetRoleClaimsByRoleIdQueryHandler : IRequestHandler<GetRoleClaimsByRoleIdQuery, IEnumerable<RoleClaimDto>>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityRepository _identityRepository;
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IPermissionRepository _permissionRepository;

        public GetRoleClaimsByRoleIdQueryHandler(IMapper mapper, IIdentityRepository identityRepository, IApiResourceRepository apiResourceRepository, IPermissionRepository permissionRepository)
        {
            _mapper = mapper;
            _identityRepository = identityRepository;
            _apiResourceRepository = apiResourceRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<RoleClaimDto>> Handle(GetRoleClaimsByRoleIdQuery request, CancellationToken cancellationToken)
        {
            var roleClaims = (_mapper.Map<IEnumerable<ApplicationUserRoleClaim>, IEnumerable<RoleClaimDto>>(
                await _identityRepository.GetRoleClaimsAsync(request.RoleId))).ToList();

            return roleClaims;
        }
    }
}
