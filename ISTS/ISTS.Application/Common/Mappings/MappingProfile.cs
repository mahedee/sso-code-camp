using AutoMapper;
using ISTS.Application.Commands.ApiResource;
using ISTS.Application.Commands.ApiScope;
using ISTS.Application.Commands.Client;
using ISTS.Application.Commands.IdentityResource;
using ISTS.Application.Commands.Permission;
using ISTS.Application.Commands.Role;
using ISTS.Application.Commands.RolePermission;
using ISTS.Application.Commands.User;
using ISTS.Application.Dtos;
using ISTS.Core.Entities;
using ISTS.Core.Entities.Identity;
using IdentityServer4.EntityFramework.Entities;

namespace ISTS.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Role Mapping

            CreateMap<ApplicationRole, RoleDto>().ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<CreateRoleCommand, ApplicationRole>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.RoleName))
                .ForMember(src => src.Id, opt => opt.Ignore())
                .ForMember(src => src.IsDeletable, opt => opt.Ignore())
                .ForMember(src => src.NormalizedName, opt => opt.Ignore())
                .ForMember(src => src.ConcurrencyStamp, opt => opt.Ignore());

            CreateMap<UpdateRoleCommand, ApplicationRole>().ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.RoleName))
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ApplicationUserRoleClaim, RoleClaimDto>()
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(x => x.Id));
            CreateMap<CreateRoleClaimCommand, ApplicationUserRoleClaim>();

            CreateMap<UpdateRoleClaimCommand, ApplicationUserRoleClaim>();

            #endregion

            #region User Mapping

            CreateMap<ApplicationUser, UserDto>().ReverseMap();
            CreateMap<CreateUserCommand, ApplicationUser>();
            CreateMap<UpdateUserCommand, ApplicationUser>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<ApplicationUserClaim, UserClaimDto>()
                .ForMember(src => src.ClaimId, opt => opt.MapFrom(des => des.Id))
                .ReverseMap();
            CreateMap<CreateUserClaimCommand, ApplicationUserClaim>();
            CreateMap<UpdateUserClaimCommand, ApplicationUserClaim>().ForMember(x => x.Id, opt => opt.MapFrom(src => src.ClaimId));

            #endregion

            #region Client Mapping

            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<CreateClientCommand, Client>().ReverseMap();
            CreateMap<UpdateClientCommand, Client>().ReverseMap();
            CreateMap<ClientClaim, ClientClaimDto>().ReverseMap();
            CreateMap<ClientProperty, ClientPropertyDto>().ReverseMap();
            CreateMap<ClientSecret, ClientSecretDto>().ReverseMap();
            CreateMap<CreateClientSecretCommand, ClientSecret>()
                .ForMember(x => x.Value, opt => opt.MapFrom(y => y.ClientSecret));

            CreateMap<ClientGrantType, string>()
                .ConstructUsing(src => src.GrantType)
                .ReverseMap()
                .ForMember(dest => dest.GrantType,
                    opt => opt.MapFrom(src => src.Trim()));

            CreateMap<ClientRedirectUri, string>()
                .ConstructUsing(src => src.RedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.RedirectUri, opt => opt.MapFrom(src => src.Trim()));

            CreateMap<ClientPostLogoutRedirectUri, string>()
                .ConstructUsing(src => src.PostLogoutRedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.PostLogoutRedirectUri, opt => opt.MapFrom(src => src.Trim()));

            CreateMap<ClientScope, string>()
                .ConstructUsing(src => src.Scope)
                .ReverseMap()
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src.Trim()));

            CreateMap<ClientIdPRestriction, string>()
                .ConstructUsing(src => src.Provider)
                .ReverseMap()
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Trim()));

            CreateMap<ClientCorsOrigin, string>()
                .ConstructUsing(src => src.Origin)
                .ReverseMap()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Trim()));

            #endregion

            #region IdentityResource Mapping

            CreateMap<IdentityResource, IdentityResourceDto>().ReverseMap();
            CreateMap<IdentityResource, CreateIdentityResourceCommand>().ReverseMap();

            CreateMap<IdentityResourceClaim, string>()
                .ConstructUsing(src => src.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            #endregion

            #region ApiResource Mapping

            CreateMap<ApiResource, ApiResourceDto>().ReverseMap();
            CreateMap<CreateApiResourceCommand, ApiResource>().ReverseMap();
            CreateMap<UpdateApiResourceCommand, ApiResource>().ReverseMap();

            CreateMap<ApiResourceClaim, string>()
                .ConstructUsing(src => src.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<ApiResourceScope, string>()
                .ConstructUsing(src => src.Scope)
                .ReverseMap()
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

            #endregion

            #region Api Scope

            CreateMap<ApiScope, ApiScopeDto>().ReverseMap();
            CreateMap<CreateApiScopeCommand, ApiScope>().ReverseMap();
            CreateMap<UpdateApiScopeCommand, ApiScope>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ApiScopeClaim, string>()
                .ConstructUsing(src => src.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            #endregion

            #region Permission

            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<CreatePermissionCommand, Permission>().ReverseMap();

            #endregion

            CreateMap<RolePermissions, RolePermissionDto>().ReverseMap();
            CreateMap<CreateRolePermissionCommandDto, RolePermissions>().ReverseMap();
        }
    }
}
