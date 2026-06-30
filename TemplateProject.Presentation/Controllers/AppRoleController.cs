using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.AppRole.Commands.Create;
using QrAssignment.Application.Features.AppRole.Commands.Delete;
using QrAssignment.Application.Features.AppRole.Commands.Update;
using QrAssignment.Application.Features.AppRole.Queries.GetList;
using QrAssignment.Application.Features.AppUser.Queries.GetList;
using QrAssignment.Application.Features.Tenants.Queries.GetList;


namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppRoleController : ControllerBase
    {
        private readonly IMediator _mediator;
         
        public AppRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/Roles/GetList
        [HttpGet("[action]")]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new GetAppRolesQuery());
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
        public async Task<IActionResult> Update([FromBody] UpdateAppRoleCommand command)
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
            var command = new DeleteAppRoleCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
