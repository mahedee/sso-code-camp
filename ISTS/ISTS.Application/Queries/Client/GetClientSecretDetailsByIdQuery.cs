using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using IdentityServer4.EntityFramework.Entities;
using MediatR;

namespace ISTS.Application.Queries.Client
{
    public class GetClientSecretDetailsByIdQuery : IRequest<ClientSecretDto>
    {
        public GetClientSecretDetailsByIdQuery(int secretId, int clientId)
        {
            SecretId = secretId;
            ClientId = clientId;
        }

        public int ClientId { get; private set; }
        public int SecretId { get; private set; }
    }

    public class GetClientSecretDetailsByIdQueryHandler : IRequestHandler<GetClientSecretDetailsByIdQuery, ClientSecretDto>
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;

        public GetClientSecretDetailsByIdQueryHandler(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<ClientSecretDto> Handle(GetClientSecretDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var secretDetails = await _clientRepository.GetClientSecretAsync(request.ClientId, request.SecretId);
            return _mapper.Map<ClientSecret, ClientSecretDto>(secretDetails);
        }
    }
}
