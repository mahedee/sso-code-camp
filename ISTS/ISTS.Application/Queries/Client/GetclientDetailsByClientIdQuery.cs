using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.Client
{
    public class GetClientDetailsByClientIdQuery : IRequest<ClientDto>
    {
        public GetClientDetailsByClientIdQuery(string clientId)
        {
            ClientId = clientId;
        }

        public string ClientId { get; private set; }
    }

    public class GetClientDetailsByClientIdQueryHandler : IRequestHandler<GetClientDetailsByClientIdQuery, ClientDto>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public GetClientDetailsByClientIdQueryHandler(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(GetClientDetailsByClientIdQuery request, CancellationToken cancellationToken)
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
