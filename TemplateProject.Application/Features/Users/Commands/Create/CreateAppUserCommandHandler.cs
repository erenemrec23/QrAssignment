using MediatR;
using QrAssignment.Application.Features.Permission.Commands.Update; // PermissionUserUpdateDto
using QrAssignment.Application.Repositories;                        // IAppUserRepository
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Create
{
    internal sealed class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand, Result>
    {
        private readonly IAuthService _authService;
        private readonly IAppUserRepository _appUserRepository;

        public CreateAppUserCommandHandler(
            IAuthService authService,
            IAppUserRepository appUserRepository)
        {
            _authService = authService;
            _appUserRepository = appUserRepository;
        }

        public async Task<Result> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            // Parola olusturma / hash / UserManager.CreateAsync sorumlulugu AuthService'te.
            // DEGISIKLIK: CreateAsync artik olusturulan kullanicinin Id'sini donuyor.
            var userId = await _authService.CreateAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                cancellationToken);


            // Yetkiler ayni istekte senkronize edilir (matris ekrani Scope=Page).
            if (request.Permissions is not null)
            {
                await _appUserRepository.SyncUserPermissionsAsync(
                    userId, request.Permissions, cancellationToken);
            }

            // Roller ayni istekte senkronize edilir.
            if (request.RoleIds is not null)
            {
                await _appUserRepository.SyncAssignedRolesAsync(
                    userId, request.RoleIds, cancellationToken);
            }

            return Result.Success();
        }
    }
}