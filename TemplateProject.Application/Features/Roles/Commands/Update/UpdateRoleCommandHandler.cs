using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.Update
{
    // Handler
    public sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result>
    {
        private readonly RoleManager<QrAssignment.Domain.Entity.App.AppRole> _roleManager;

        public UpdateRoleCommandHandler(RoleManager<QrAssignment.Domain.Entity.App.AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            if (role == null)
            {
                return Result.Failure(new Error("Güncellenecek rol bulunamadı.",""));
            }

            // Eğer isim değişiyorsa ve yeni isim başka bir rolde kullanılıyorsa çakışmayı önle
            if (role.Name != request.Name && await _roleManager.RoleExistsAsync(request.Name))
            {
                return Result.Failure(new Error("Bu isimde başka bir rol zaten mevcut.",""));
            }

            role.Name = request.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(new Error($"Rol güncellenirken hata oluştu: {errors}",""));
            }

            return Result.Success("Rol başarıyla güncellendi.");
        }
    }
}