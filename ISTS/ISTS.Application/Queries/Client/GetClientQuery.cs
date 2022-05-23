using AutoMapper;
using ISTS.Application.Common.Interfaces.Identity;
using ISTS.Application.Dtos;
using MediatR;

namespace ISTS.Application.Queries.Client
{
    public class GetClientQuery : IRequest<IEnumerable<ClientDto>>
    {
        public string SearchText { get; set; }
    }

    public class GetClientQueryHandler : IRequestHandler<GetClientQuery, IEnumerable<ClientDto>>
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;

        public GetClientQueryHandler(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<ClientDto>> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<IdentityServer4.EntityFramework.Entities.Client>, IEnumerable<ClientDto>>
                (await _clientRepository.GetClientsAsync(request.SearchText));
        }
    }
}
