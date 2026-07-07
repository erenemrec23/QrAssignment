using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Users.Commands.Create;
using QrAssignment.Application.Features.Users.Commands.Update;
using QrAssignment.Application.Features.Users.Queries.GetById;
using QrAssignment.Application.Features.Users.Queries.GetList;


namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Sadece IMediator'ı enjekte ediyoruz, AuthService veya JwtProvider ile işimiz yok!
        public AppUserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetList([FromQuery] GetUserListQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid? id, CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
