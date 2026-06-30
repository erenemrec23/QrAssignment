using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text; 

namespace QrAssignment.Application.Features.Permission.Commands.Update
{
    internal sealed class UpdateUserPermissionsCommandHandler
        : IRequestHandler<UpdateUserPermissionsCommand, Result>
    {
        private readonly UserManager<Domain.Entity.App.AppUser> _userManager;

        public UpdateUserPermissionsCommandHandler(UserManager<Domain.Entity.App.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(UpdateUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcıyı bul
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Result.Failure(new Error("UserNotFound", "Kullanıcı bulunamadı."));
            }

            // 2. Kullanıcının mevcut tüm Claim'lerini (Haklarını) getir
            var existingClaims = await _userManager.GetClaimsAsync(user);

            // 3. Sadece yetkiyle ilgili olanları (Page_ ile başlayanları) bul ve veritabanından sil
            // (Böylece seçimi kaldırılan checkbox'lar da temizlenmiş olur)
            var permissionClaimsToRemove = existingClaims.Where(c => c.Type.StartsWith("Page_")).ToList();
            if (permissionClaimsToRemove.Any())
            {
                var removeResult = await _userManager.RemoveClaimsAsync(user, permissionClaimsToRemove);
                if (!removeResult.Succeeded)
                {
                    return Result.Failure(new Error("ClaimRemoveFailed", "Eski yetkiler temizlenirken bir hata oluştu."));
                }
            }

            // 4. Arayüzden gelen yeni yetki listesini Claim nesnelerine dönüştür
            // Yetkisi 0 (None) olanları veritabanına yazıp kalabalık yapmaya gerek yok
            var newClaims = request.Permissions
                .Where(p => p.PermissionValue > 0)
                .Select(p => new Claim(p.PageName, p.PermissionValue.ToString()))
                .ToList();

            // 5. Yeni yetkileri veritabanına kaydet
            if (newClaims.Any())
            {
                var addResult = await _userManager.AddClaimsAsync(user, newClaims);
                if (!addResult.Succeeded)
                {
                    return Result.Failure(new Error("ClaimAddFailed", "Yeni yetkiler eklenirken bir hata oluştu."));
                }
            }

            return Result.Success();
        }
    }
}
