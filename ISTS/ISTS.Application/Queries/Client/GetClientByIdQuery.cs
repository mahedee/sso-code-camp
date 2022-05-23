using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.Client
{
    public class GetClientByIdQuery : IRequest<ClientDto>
    {
        public GetClientByIdQuery(int clientId)
        {
            ClientId = clientId;
        }

        public int ClientId { get; private set; }
    }

    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto>
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;

        public GetClientByIdQueryHandler(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<ClientDto> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            var clientDetails =
                await _clientRepository.GetClientAsync(request.ClientId);
            if (clientDetails == null)
            {
                throw new NotFoundException("Client information not found");
            }

            var clientAdditionalDetails = await _clientRepository.GetClientAdditionalDetailsAsync(clientDetails.Id);
            var mappedResult = _mapper.Map<IdentityServer4.EntityFramework.Entities.Client, ClientDto>(clientDetails);
            if (clientAdditionalDetails != null)
            {
                mappedResult.ClientSecret = clientAdditionalDetails.ClientSecret;
            }

            return mappedResult;
        }
    }
}
