using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.Client
{
    public class DeleteClientCommand : IRequest
    {
        public DeleteClientCommand(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }

    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Unit>
    {
        private readonly IClientRepository _clientRepository;

        public DeleteClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var clientDetails = await _clientRepository.GetClientAsync(request.Id);
            if (clientDetails == null)
            {
                throw new NotFoundException("Client Information not found");
            }

            await _clientRepository.RemoveClientAsync(clientDetails);

            return Unit.Value;
        }
    }
}
