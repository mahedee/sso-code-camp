using ISTS.API.Filters;
using ISTS.Application.Commands.Role;
using ISTS.Application.Commands.RolePermission;
using ISTS.Application.Common.Constants;
using ISTS.Application.Dtos;
using ISTS.Application.Queries.Role;
using ISTS.Application.Queries.RolePermission;
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

        [Permission(Permission = PermissionConstants.ReadRoles)]
        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<RoleDto>))]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [Permission(Permission = PermissionConstants.CreateRoles)]
        [HttpPost]
        [ProducesDefaultResponseType(typeof(RoleDto))]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
        {
            var createdRole = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetRole), new { roleId = createdRole.Id }, createdRole);
        }

        [Permission(Permission = PermissionConstants.UpdateRoles)]
        [HttpPut]
        [ProducesDefaultResponseType(typeof(RoleDto))]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Permission(Permission = PermissionConstants.DeleteRoles)]
        [HttpDelete("{roleId}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            return Ok(await _mediator.Send(new DeleteRoleCommand(roleId)));
        }

        [Permission(Permission = PermissionConstants.ReadRoles)]
        [HttpGet("{roleId}/users")]
        [ProducesDefaultResponseType(typeof(IEnumerable<UserDto>))]
        public async Task<IActionResult> GetRoleUsers(string roleId)
        {
            return Ok(await _mediator.Send(new GetUsersByRoleIdQuery() { RoleId = roleId }));
        }

        [Permission(Permission = PermissionConstants.ReadRoles)]
        [HttpGet("{roleId}/claims")]
        [ProducesDefaultResponseType(typeof(IEnumerable<RoleClaimDto>))]
        public async Task<IActionResult> GetRoleClaims(string roleId, int page = 1, int pageSize = 10)
        {
            return Ok(await _mediator.Send(new GetRoleClaimsByRoleIdQuery() { RoleId = roleId }));
        }

        [Permission(Permission = PermissionConstants.UpdateRoles)]
        [HttpPost("claim")]
        [ProducesDefaultResponseType(typeof(RoleClaimDto))]
        public async Task<IActionResult> CreateRoleClaim([FromBody] CreateRoleClaimCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [Permission(Permission = PermissionConstants.UpdateRoles)]
        [HttpPut("claim/{claimId:int}")]
        public async Task<IActionResult> UpdateRoleClaim(int claimId, [FromBody] UpdateRoleClaimCommand command)
        {
            command.Id = claimId;
            await _mediator.Send(command);
            return Ok();
        }

        [Permission(Permission = PermissionConstants.DeleteRoles)]
        [HttpDelete("{roleId}/claim/{claimId:int}")]
        public async Task<IActionResult> DeleteRoleClaim(string roleId, int claimId)
        {
            return Ok(await _mediator.Send(new DeleteRoleClaimCommand() { RoleId = roleId, ClaimId = claimId }));
        }

        [HttpGet("{roleId}/role-permission")]
        public async Task<IActionResult> GetRolePermissionsAsync(string roleId)
        {
            return Ok(await _mediator.Send(new GetRolePermissionsQuery(roleId)));
        }

        [HttpPost("role-permission")]
        public async Task<IActionResult> AssignRolePermissionAsync([FromBody] CreateRolePermissionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{roleId}/role-permission/{permissionId:int}")]
        public async Task<IActionResult> DeleteRolePermissionAsync(string roleId, int permissionId)
        {
            return Ok(await _mediator.Send(new DeleteRolePermissionCommand(roleId, permissionId)));
        }

        [HttpDelete("{roleId}/role-permission/")]
        public async Task<IActionResult> DeleteRolePermissionAsync(string roleId)
        {
            return Ok(await _mediator.Send(new DeleteRolePermissionsByRoleIdCommand(roleId)));
        }
    }
}
