using System.Threading.Tasks;
using ISTS.Application.Commands.IdentityResource;
using ISTS.Application.Queries.IdentityResource;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityResourcesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IdentityResourcesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetIdentityResources([FromQuery] GetIdentityResourcesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{identityResourceId:int}")]
        public async Task<IActionResult> GetIdentityResourceDetails(int identityResourceId)
        {
            var query = new GetIdentityResourceByIdQuery(identityResourceId);
            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> CreateIdentityResource([FromBody] CreateIdentityResourceCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{identityResourceId:int}")]
        public async Task<IActionResult> DeleteIdentityResource(int identityResourceId)
        {
            return Ok(await _mediator.Send(new DeleteIdentityResourceCommand(identityResourceId)));
        }
    }
}
