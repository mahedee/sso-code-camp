using ISTS.Application.Common.Exceptions;
using ISTS.Application.Common.Interfaces.Identity;
using MediatR;

namespace ISTS.Application.Commands.ApiScope
{
    public class DeleteApiScopeCommand : IRequest<int>
    {
        public DeleteApiScopeCommand(int apiScopeId)
        {
            ApiScopeId = apiScopeId;
        }

        public int ApiScopeId { get; private set; }
    }

    public class DeleteApiScopeCommandHandler : IRequestHandler<DeleteApiScopeCommand, int>
    {
        private readonly IApiScopeRepository _apiScopeRepository;

        public DeleteApiScopeCommandHandler(IApiScopeRepository apiScopeRepository)
        {
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<int> Handle(DeleteApiScopeCommand request, CancellationToken cancellationToken)
        {
            var apiScopeDetails = await _apiScopeRepository.GetApiScopeAsync(request.ApiScopeId);
            if (apiScopeDetails == null)
                throw new NotFoundException("Api Scope not found");
            var id = await _apiScopeRepository.DeleteApiScopeAsync(apiScopeDetails);
            return id;
        }
    }
}
