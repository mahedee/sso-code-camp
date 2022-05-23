using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Core.Enums;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using MediatR;

namespace ISTS.Application.Commands.Client
{
    public class CreateClientSecretCommand : IRequest<int>
    {
        public int ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Description { get; set; }
        public DateTime? Expired { get; set; }
        public HashType HashType { get; set; } = HashType.Sha256;
    }

    public class CreateClientSecretCommandHandler : IRequestHandler<CreateClientSecretCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;
        private const string SharedSecret = "SharedSecret";
        public CreateClientSecretCommandHandler(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<int> Handle(CreateClientSecretCommand request, CancellationToken cancellationToken)
        {
            var clientInfo = await _clientRepository.GetClientAsync(request.ClientId);
            if (clientInfo == null) throw new NotFoundException("Client Information not found");

            PrepareClientSharedSecret(request);
            var clientSecret = _mapper.Map<CreateClientSecretCommand, ClientSecret>(request);
            clientSecret.Created = DateTime.Now;
            clientSecret.Type = SharedSecret;

            return await _clientRepository.AddClientSecretAsync(request.ClientId, clientSecret);
        }

        private void PrepareClientSharedSecret(CreateClientSecretCommand clientSecret)
        {
            if (clientSecret.HashType == HashType.Sha256)
            {
                clientSecret.ClientSecret = clientSecret.ClientSecret.Sha256();
            }
            if (clientSecret.HashType == HashType.Sha512)
            {
                clientSecret.ClientSecret = clientSecret.ClientSecret.Sha512();
            }
        }
    }
}
