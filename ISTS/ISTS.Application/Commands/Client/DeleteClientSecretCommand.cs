using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.Client
{
    public class DeleteClientSecretCommand : IRequest<int>
    {
        public DeleteClientSecretCommand(int secretId, int clientId)
        {
            SecretId = secretId;
            ClientId = clientId;
        }

        public int SecretId { get; private set; }
        public int ClientId { get; private set; }
    }

    public class DeleteClientSecretCommandHandler : IRequestHandler<DeleteClientSecretCommand, int>
    {
        private readonly IClientRepository _clientRepository;

        public DeleteClientSecretCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<int> Handle(DeleteClientSecretCommand request, CancellationToken cancellationToken)
        {
            var result =
                await _clientRepository.DeleteClientSecretAsync(
                    await _clientRepository.GetClientSecretAsync(request.ClientId, request.SecretId));
            return result;
        }
    }
}
