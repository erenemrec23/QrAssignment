using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Roles.Commands.Create;
using QrAssignment.Application.Features.Roles.Commands.Delete;
using QrAssignment.Application.Features.Roles.Commands.Update;
using QrAssignment.Application.Features.Roles.Queries.GetById;
using QrAssignment.Application.Features.Roles.Queries.GetList;
using QrAssignment.Application.Features.Tenants.Queries.GetById;


namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppRolesController : ControllerBase
    {
        private readonly IMediator _mediator;
         
        public AppRolesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/Roles/GetList
        [HttpGet("[action]")]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetListRoleQuery());
            return Ok(result);
        }

        // POST: api/Roles/Create
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // PUT: api/Roles/Update
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
         
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteRoleCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
         

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid? id, CancellationToken cancellationToken)
        {
            var query = new GetByIdRoleQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
