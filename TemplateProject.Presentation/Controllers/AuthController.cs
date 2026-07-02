using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.AppUser.Commands.Create;
using QrAssignment.Application.Features.AuthFeatures.Commands.Login;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public sealed class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Sadece IMediator'ı enjekte ediyoruz, AuthService veya JwtProvider ile işimiz yok!
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        { 
            var response = await _mediator.Send(command, cancellationToken);
             
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateAppUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
