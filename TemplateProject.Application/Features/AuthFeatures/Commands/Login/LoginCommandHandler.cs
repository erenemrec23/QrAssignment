using MediatR;
using TemplateProject.Application.Services;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.AuthFeatures.Commands.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(request.UserNameOrEmail, request.Password, cancellationToken);

            return Result.Success(result);
        }
    }
}
