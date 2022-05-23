using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.Permission
{
    public class GetPermissionByApiResourceIdQuery : IRequest<IEnumerable<PermissionDto>>
    {
        public GetPermissionByApiResourceIdQuery(int apiResourceId)
        {
            ApiResourceId = apiResourceId;
        }

        public int ApiResourceId { get; private set; }
    }

    public class GetPermissionByClientIdQueryHandler : IRequestHandler<GetPermissionByApiResourceIdQuery, IEnumerable<PermissionDto>>
    {
        private readonly IMapper _mapper;
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionByClientIdQueryHandler(IMapper mapper, IPermissionRepository permissionRepository)
        {
            _mapper = mapper;
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionByApiResourceIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<Core.Entities.Permission>, IEnumerable<PermissionDto>>(
                await _permissionRepository.GetPermissionsByApiResourceId(request.ApiResourceId));
        }
    }
}
