using System.Collections.Generic;
using System.Threading.Tasks;
using ISTS.API.Filters;
using ISTS.Application.Commands.ApiScope;
using ISTS.Application.Common.Constants;
using ISTS.Application.Dtos;
using ISTS.Application.Queries.ApiScope;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISTS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiScopesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiScopesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<ApiScopeDto>))]
        public async Task<IActionResult> GetApiScopesAsync([FromQuery] GetApiScopeQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{apiScopeId:int}")]
        [ProducesDefaultResponseType(typeof(ApiScopeDto))]
        public async Task<IActionResult> GetApiScopeDetailsAsync(int apiScopeId)
        {
            return Ok(await _mediator.Send(new GetApiScopeByIdQuery(apiScopeId)));
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(ApiScopeDto))]
        public async Task<IActionResult> CreateApiScopeAsync([FromBody] CreateApiScopeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{apiScopeId:int}")]
        [ProducesDefaultResponseType(typeof(ApiScopeDto))]
        public async Task<IActionResult> UpdateApiScopeAsync(int apiScopeId, [FromBody] UpdateApiScopeCommand command)
        {
            command.Id = apiScopeId;
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{apiScopeId:int}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> DeleteApiScopeAsync(int apiScopeId)
        {
            return Ok(await _mediator.Send(new DeleteApiScopeCommand(apiScopeId)));
        }
    }
}
