using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.QrLocations.Commands.Create;
using QrAssignment.Application.Features.QrLocations.Commands.Update;
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
        [HttpPost("[action]")]
        public async Task<IActionResult> Update(UpdateQrLocationCommand command)
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
    }
}
