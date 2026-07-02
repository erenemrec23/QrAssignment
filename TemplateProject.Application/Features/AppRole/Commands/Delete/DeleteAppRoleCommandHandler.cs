using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Shared; 
namespace QrAssignment.Application.Features.AppRole.Commands.Delete
{
    // Handler
    public sealed class DeleteAppRoleCommandHandler : IRequestHandler<DeleteAppRoleCommand, Result>
    {
        private readonly RoleManager<
 QrAssignment.Domain.Entity.App.AppRole> _roleManager;

        public DeleteAppRoleCommandHandler(RoleManager<QrAssignment.Domain.Entity.App.AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result> Handle(DeleteAppRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);
            if (role == null)
            {
                return Result.Failure(new Error("Silinecek rol bulunamadı.",""));
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(new Error($"Rol silinirken hata oluştu: {errors}",""));
            }

            return Result.Success("Rol başarıyla silindi.");
        }
    }
}