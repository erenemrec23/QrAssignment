using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Roles.Commands.Create;
using QrAssignment.Application.Features.Roles.Commands.Delete;
using QrAssignment.Application.Features.Roles.Commands.Update;
using QrAssignment.Application.Features.Roles.Queries.GetList;


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
            var result = await _mediator.Send(new GetRoleListQuery());
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

        // DELETE: api/Roles/Delete
        // Delete işleminde genellikle ID URL üzerinden (Route parameter) alınır.
        // Eğer Command nesnesini body'den göndermek istersen [FromBody] olarak değiştirebilirsin.
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteRoleCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
