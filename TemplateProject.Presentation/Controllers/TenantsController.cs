using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Tenants.Commands.Create;
using QrAssignment.Application.Features.Tenants.Commands.Delete;
using QrAssignment.Application.Features.Tenants.Commands.Update;
using QrAssignment.Application.Features.Tenants.Queries.GetById;
using QrAssignment.Application.Features.Tenants.Queries.GetList;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TenantsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateTenantCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateTenantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            // 1. Gelen ID'yi Command nesnemizin içine koyuyoruz
            var command = new DeleteTenantCommand { Id = id };

            // 2. MediatR'a komutu gönderip Handler'ın çalışmasını bekliyoruz
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPost("GetList")]  
        public async Task<IActionResult> GetList([FromBody] GetListTenantQuery request) // 2. FromQuery yerine FromBody yapıyoruz
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new TenantGetByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
