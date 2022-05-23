using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using IdentityServer4.EntityFramework.Entities;
using MediatR;

namespace ISTS.Application.Queries.Client
{
    public class GetClientSecretsByClientIdQuery : IRequest<IEnumerable<ClientSecretDto>>
    {
        public GetClientSecretsByClientIdQuery(int clientId)
        {
            ClientId = clientId;
        }

        public int ClientId { get; private set; }
    }

    public class GetClientSecretsByClientIdQueryHandler : IRequestHandler<GetClientSecretsByClientIdQuery, IEnumerable<ClientSecretDto>>
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;

        public GetClientSecretsByClientIdQueryHandler(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<ClientSecretDto>> Handle(GetClientSecretsByClientIdQuery request, CancellationToken cancellationToken)
        {
            var secrets = await _clientRepository.GetClientSecretsAsync(request.ClientId);
            return _mapper.Map<IEnumerable<ClientSecret>, IEnumerable<ClientSecretDto>>(secrets);
        }
    }
}
