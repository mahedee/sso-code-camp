using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.IdentityResource
{
    public class GetIdentityResourceByIdQuery : IRequest<IdentityResourceDto>
    {
        public GetIdentityResourceByIdQuery(int identityResourceId)
        {
            IdentityResourceId = identityResourceId;
        }

        public int IdentityResourceId { get; private set; }
    }
    public class GetIdentityResourceByIdQueryHandler : IRequestHandler<GetIdentityResourceByIdQuery, IdentityResourceDto>
    {
        private readonly IMapper _mapper;
        private readonly IIdentityResourceRepository _identityResourceRepository;

        public GetIdentityResourceByIdQueryHandler(IMapper mapper, IIdentityResourceRepository identityResourceRepository)
        {
            _mapper = mapper;
            _identityResourceRepository = identityResourceRepository;
        }

        public async Task<IdentityResourceDto> Handle(GetIdentityResourceByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IdentityServer4.EntityFramework.Entities.IdentityResource, IdentityResourceDto>(
                await _identityResourceRepository.GetIdentityResourceAsync(request.IdentityResourceId));
        }
    }
}
