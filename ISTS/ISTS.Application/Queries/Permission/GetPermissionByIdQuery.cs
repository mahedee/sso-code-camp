using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.Permission
{
    public class GetPermissionByIdQuery : IRequest<PermissionDto>
    {
        public GetPermissionByIdQuery(int permissionId)
        {
            PermissionId = permissionId;
        }

        public int PermissionId { get; private set; }
    }

    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto>
    {
        private readonly IMapper _mapper;
        private readonly IPermissionRepository _permissionRepository;

        public GetPermissionByIdQueryHandler(IMapper mapper, IPermissionRepository permissionRepository)
        {
            _mapper = mapper;
            _permissionRepository = permissionRepository;
        }

        public async Task<PermissionDto> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<Core.Entities.Permission, PermissionDto>(
                await _permissionRepository.GetPermissionDetails(request.PermissionId) ?? new Core.Entities.Permission());
        }
    }
}
