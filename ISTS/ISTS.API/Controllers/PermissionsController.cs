using System.Threading.Tasks;
using ISTS.Application.Commands.Permission;
using ISTS.Application.Queries.Permission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISTS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{apiResourceId:int}")]
        public async Task<IActionResult> GetPermissionsAsync(int apiResourceId)
        {
            return Ok(await _mediator.Send(new GetPermissionByApiResourceIdQuery(apiResourceId)));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermissionAsync([FromBody] CreatePermissionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{permissionId:int}")]
        public async Task<IActionResult> DeletePermissionAsync(int permissionId)
        {
            return Ok(await _mediator.Send(new DeletePermissionCommand(permissionId)));
        }
    }
}
