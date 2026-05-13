using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateProject.Application.Features.AppUser.Commands.CreateAppUser;
using TemplateProject.Application.Features.AuthFeatures.Commands.Login;

namespace TemplateProject.Presentation.Controllers
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
        public async Task<IActionResult> Register([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
