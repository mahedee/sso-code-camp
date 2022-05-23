using System.Collections.Generic;
using System.Threading.Tasks;
using ISTS.API.Filters;
using ISTS.Application.Commands.ApiResource;
using ISTS.Application.Common.Constants;
using ISTS.Application.Dtos;
using ISTS.Application.Queries.ApiResource;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISTS.API.Controllers
{
    /// <summary>
    /// Manage API Resources
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiResourcesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiResourcesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get All API Resources from Database
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Permission(Permission = PermissionConstants.ReadApiResources)]
        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<ApiResourceDto>))]
        public async Task<IActionResult> GetApiResources([FromQuery] GetApiResourcesQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        /// <summary>
        /// Get API Resource information by ID
        /// </summary>
        /// <param name="apiResourceId"></param>
        /// <returns></returns>
        [Permission(Permission = PermissionConstants.ReadApiResources)]
        [HttpGet("{apiResourceId:int}")]
        [ProducesDefaultResponseType(typeof(ApiResourceDto))]
        public async Task<IActionResult> GetApiResourceDetails(int apiResourceId)
        {
            return Ok(await _mediator.Send(new GetApiResourceDetailsByIdQuery(apiResourceId)));
        }

        /// <summary>
        /// Create API Resource
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Permission(Permission = PermissionConstants.CreateApiResources)]
        [HttpPost]
        [ProducesDefaultResponseType(typeof(ApiResourceDto))]
        public async Task<IActionResult> CreateApiResource([FromBody] CreateApiResourceCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Update API Resource Information
        /// </summary>
        /// <param name="apiResourceId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [Permission(Permission = PermissionConstants.UpdateApiResources)]
        [HttpPut("{apiResourceId:int}")]
        [ProducesDefaultResponseType(typeof(ApiResourceDto))]
        public async Task<IActionResult> UpdateApiResource(int apiResourceId, [FromBody] UpdateApiResourceCommand command)
        {
            command.Id = apiResourceId;
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete API Resource Infromation
        /// </summary>
        /// <param name="apiResourceId"></param>
        /// <returns></returns>
        [Permission(Permission = PermissionConstants.DeleteApiResources)]
        [HttpDelete("{apiResourceId:int}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> DeleteApiResource(int apiResourceId)
        {
            return Ok(await _mediator.Send(new DeleteApiResourceCommand(apiResourceId)));
        }
    }
}
