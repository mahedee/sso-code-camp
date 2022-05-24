using System.Collections.Generic;
using System.Threading.Tasks;
using ISTS.API.Filters;
using ISTS.Application.Commands.Client;
using ISTS.Application.Common.Constants;
using ISTS.Application.Dtos;
using ISTS.Application.Queries.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISTS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Permission(Permission = PermissionConstants.ReadClients)]
        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<ClientDto>))]
        public async Task<IActionResult> GetClients([FromQuery] GetClientQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        //[Permission(Permission = PermissionConstants.ReadClients)]
        [HttpGet("{clientId:int}")]
        [ProducesDefaultResponseType(typeof(ClientDto))]
        public async Task<IActionResult> GetClient(int clientId)
        {
            return Ok(await _mediator.Send(new GetClientByIdQuery(clientId)));
        }

       // [Permission(Permission = PermissionConstants.ReadClients)]
        [HttpGet("details-by-client-id/{clientId}")]
        [ProducesDefaultResponseType(typeof(ClientDto))]
        public async Task<IActionResult> GetClient(string clientId)
        {
            return Ok(await _mediator.Send(new GetClientDetailsByClientIdQuery(clientId)));
        }

        //[Permission(Permission = PermissionConstants.CreateClients)]
        [HttpPost]
        [ProducesDefaultResponseType(typeof(ClientDto))]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        //[Permission(Permission = PermissionConstants.UpdateClients)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        //[Permission(Permission = PermissionConstants.DeleteClients)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            return Ok(await _mediator.Send(new DeleteClientCommand(id)));
        }

        //[HttpGet("{clientId:int}/Secrets")]
        //[ProducesDefaultResponseType(typeof(IEnumerable<ClientSecretDto>))]
        //public async Task<IActionResult> GetSecrets(int clientId)
        //{
        //    return Ok(await _mediator.Send(new GetClientSecretsByClientIdQuery(clientId)));
        //}

        //[HttpGet("{clientId:int}/Secrets/{secretId:int}")]
        //public async Task<IActionResult> GetSecretDetails(int clientId, int secretId)
        //{
        //    return Ok(await _mediator.Send(new GetClientSecretDetailsByIdQuery(secretId, clientId)));
        //}

        //[HttpPost("{clientId:int}/Secrets")]
        //public async Task<IActionResult> CreateSecret(int clientId, [FromBody] CreateClientSecretCommand command)
        //{
        //    command.ClientId = clientId;
        //    return Ok(await _mediator.Send(command));
        //}

        //[HttpDelete("{clientId:int}/Secrets/{secretId:int}")]
        //public async Task<IActionResult> DeleteSecret(int clientId, int secretId)
        //{
        //    return Ok(await _mediator.Send(new DeleteClientSecretCommand(secretId, clientId)));
        //}

        //TODO: Client Properties and Client Claims CRUD needs to be implemented
    }
}
