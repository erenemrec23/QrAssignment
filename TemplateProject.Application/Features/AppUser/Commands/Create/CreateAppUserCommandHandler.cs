using MediatR;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AppUser.Commands.Create
{
    internal sealed class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand, Result<Unit>>
    {
        private readonly IAuthService _authService;

        public CreateAppUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<Unit>> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            await _authService.CreateAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
