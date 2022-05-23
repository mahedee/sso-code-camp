using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using ISTS.Core.Entities;
using MediatR;

namespace ISTS.Application.Queries.RolePermission
{
    public class GetRolePermissionsQuery : IRequest<IEnumerable<RolePermissionDto>>
    {
        public GetRolePermissionsQuery(string roleId)
        {
            RoleId = roleId;
        }

        public string RoleId { get; private set; }
    }

    public class GetRolePermissionQueryHandler : IRequestHandler<GetRolePermissionsQuery, IEnumerable<RolePermissionDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRolePermissionsRepository _rolePermissionsRepository;
        private readonly IApiResourceRepository _apiResourceRepository;
        public GetRolePermissionQueryHandler(IMapper mapper, IRolePermissionsRepository rolePermissionsRepository, IApiResourceRepository apiResourceRepository)
        {
            _mapper = mapper;
            _rolePermissionsRepository = rolePermissionsRepository;
            _apiResourceRepository = apiResourceRepository;
        }

        public async Task<IEnumerable<RolePermissionDto>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var rolePermissions =
                await _rolePermissionsRepository.GetRolePermissions(request.RoleId);

            var permissions =
                (_mapper.Map<IEnumerable<RolePermissions>, IEnumerable<RolePermissionDto>>(rolePermissions)).ToList();

            foreach (var rolePermission in permissions)
            {
                var apiResourceInfo = await _apiResourceRepository.GetApiResourceAsync(rolePermission.ApiResourceId);
                rolePermission.ApiResourceName = apiResourceInfo.Name;
                rolePermission.ApiResourceDisplayName = apiResourceInfo.DisplayName;
            }

            return permissions;
        }
    }
}
