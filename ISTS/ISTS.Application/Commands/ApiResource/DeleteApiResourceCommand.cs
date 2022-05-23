using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.ApiResource
{
    public class DeleteApiResourceCommand : IRequest<int>
    {
        public DeleteApiResourceCommand(int apiResourceId)
        {
            ApiResourceId = apiResourceId;
        }

        public int ApiResourceId { get; private set; }
    }

    public class DeleteApiResourceCommandHandler : IRequestHandler<DeleteApiResourceCommand, int>
    {
        private readonly IApiResourceRepository _apiResourceRepository;

        public DeleteApiResourceCommandHandler(IApiResourceRepository apiResourceRepository)
        {
            _apiResourceRepository = apiResourceRepository;
        }

        public async Task<int> Handle(DeleteApiResourceCommand request, CancellationToken cancellationToken)
        {
            var apiResource = await _apiResourceRepository.GetApiResourceAsync(request.ApiResourceId);
            if (apiResource == null) throw new NotFoundException("Api Resource information not found");

            return await _apiResourceRepository.DeleteApiResourceAsync(apiResource);
        }
    }
}
