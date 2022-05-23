using ISTS.API.Filters;
using ISTS.Application.Common.Constants;
using ISTS.Application.Dtos;
using ISTS.Application.Queries.Role;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Permission(Permission = PermissionConstants.ReadRoles)]
        [HttpGet("{roleId}")]
        [ProducesDefaultResponseType(typeof(RoleDto))]
        public async Task<IActionResult> GetRole(string roleId)
        {
            return Ok(await _mediator.Send(new GetRoleByIdQuery() { Id = roleId }));
        }
    }
}
