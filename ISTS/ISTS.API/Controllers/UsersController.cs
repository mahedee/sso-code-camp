using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using ISTS.API.Filters;
using ISTS.Application.Commands.User;
using ISTS.Application.Common.Constants;
using ISTS.Application.Dtos;
using ISTS.Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Permission(Permission = PermissionConstants.ReadUsers)]
        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<UserDto>))]
        public async Task<IActionResult> GetUsers([FromQuery] GetUserQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [Permission(Permission = PermissionConstants.ReadUsers)]
        [HttpGet("{userId}")]
        [ProducesDefaultResponseType(typeof(UserDto))]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            return Ok(await _mediator.Send(new GetUserByIdQuery() { UserId = userId }));
        }

        [Permission(Permission = PermissionConstants.CreateUsers)]
        [HttpPost]
        [ProducesDefaultResponseType(typeof(UserDto))]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var user = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserDetails), new { userId = user.Id }, user);
        }

        [Permission(Permission = PermissionConstants.UpdateUsers)]
        [HttpPut("{userId}")]
        [ProducesDefaultResponseType(typeof(UserDto))]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserCommand command)
        {
            command.Id = userId;
            return Ok(await _mediator.Send(command));
        }

        [Permission(Permission = PermissionConstants.DeleteUsers)]
        [HttpDelete("{userId}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (IsDeleteForbidden(userId))
            {
                return StatusCode((int)System.Net.HttpStatusCode.Forbidden);
            }
            //TODO: We might need to implement soft deletion.
            return Ok(await _mediator.Send(new DeleteUserCommand(userId)));
        }

        private bool IsDeleteForbidden(string id)
        {
            var userId = User.FindFirst(JwtClaimTypes.Subject);

            return userId != null && userId.Value == id.ToString();
        }

        [Permission(Permission = PermissionConstants.ReadUsers)]
        [HttpGet("{userId}/roles")]
        [ProducesDefaultResponseType(typeof(IEnumerable<RoleDto>))]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            return Ok(await _mediator.Send(new GetRolesByUserIdQuery(userId)));
        }

        [Permission(Permission = PermissionConstants.UpdateUsers)]
        [HttpPost("roles")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> AssignUserToRoles([FromBody] AssignUserToRoleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Permission(Permission = PermissionConstants.DeleteUsers)]
        [HttpDelete("roles")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> DeleteUserRole([FromBody] DeleteUserRoleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Permission(Permission = PermissionConstants.ReadUsers)]
        [HttpGet("{userId}/claims")]
        [ProducesDefaultResponseType(typeof(IEnumerable<UserClaimDto>))]
        public async Task<IActionResult> GetUserClaims(string userId)
        {
            return Ok(await _mediator.Send(new GetClaimsByUserIdQuery(userId)));
        }

        [Permission(Permission = PermissionConstants.UpdateUsers)]
        [HttpPost("claims")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> CreateUserClaim([FromBody] CreateUserClaimCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Permission(Permission = PermissionConstants.UpdateUsers)]
        [HttpPut("claims")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> UpdateUserClaim([FromBody] UpdateUserClaimCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [Permission(Permission = PermissionConstants.DeleteUsers)]
        [HttpDelete("{userId}/claims/{claimId:int}")]
        public async Task<IActionResult> DeleteUserClaim(string userId, int claimId)
        {
            return Ok(await _mediator.Send(new DeleteUserClaimCommand(userId, claimId)));
        }

        [Permission(Permission = PermissionConstants.UpdateUsers)]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserPasswordChangeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        //TODO: Login Providers implementation
    }
}
