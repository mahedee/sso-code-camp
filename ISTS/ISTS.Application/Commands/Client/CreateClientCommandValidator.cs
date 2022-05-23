using AutoMapper;
using FluentValidation;
using ISTS.Application.Common.Interfaces.Identity;

namespace ISTS.Application.Commands.Client
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        public CreateClientCommandValidator(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            RuleFor(x => x.ClientName).NotNull().NotEmpty();
        }
    }
}
