using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.QrLocations.Commands.Create;
using QrAssignment.Application.Features.QrLocations.Commands.Update;
using QrAssignment.Application.Features.QrLocations.Queries.GetById;
using QrAssignment.Application.Features.QrLocations.Queries.GetList;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QrLocationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QrLocationsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateQrLocationCommand command)
        { 
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateQrLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetList([FromQuery] GetListQrLocationQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new QrLocationGetByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);  

            return Ok(result);
        }
    }
}
