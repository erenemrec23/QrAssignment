using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Shared; 
namespace QrAssignment.Application.Features.Roles.Commands.Delete
{
    // Handler
    public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result>
    {
        private readonly RoleManager<
 QrAssignment.Domain.Entity.App.AppRole> _roleManager;

        public DeleteRoleCommandHandler(RoleManager<QrAssignment.Domain.Entity.App.AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
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