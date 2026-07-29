using MediatR;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Create
{
    internal sealed class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand, Result>
    {
        private readonly IAuthService _authService;

        public CreateAppUserCommandHandler(IAuthService authService)
            => _authService = authService;

        public async Task<Result> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            // Parola olusturma / hash / UserManager.CreateAsync sorumlulugu AuthService'te kaliyor.
            // Email/UserName mukerrer kontrolu ve SQL kaynakli hatalar merkezi olarak
            // GlobalExceptionHandler tarafindan ele aliniyor (try/catch yok).
            await _authService.CreateAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                cancellationToken);

            return Result.Success();
        }
    }
}
