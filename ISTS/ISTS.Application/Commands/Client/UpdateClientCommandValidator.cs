using AutoMapper;
using FluentValidation;
using ISTS.Application.Common.Interfaces.Identity;

namespace ISTS.Application.Commands.Client
{
    public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        public UpdateClientCommandValidator(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
            RuleFor(x => x.ClientId)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (model, clientId, cancellationToken) => await _clientRepository.IsClientAlreadyExists(
                    _mapper.Map<UpdateClientCommand, IdentityServer4.EntityFramework.Entities.Client>(model))).WithMessage("Client is Already Exists");
            RuleFor(x => x.ClientName).NotNull().NotEmpty();
        }
    }
}
