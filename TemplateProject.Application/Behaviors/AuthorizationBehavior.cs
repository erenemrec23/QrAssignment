using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using QrAssignment.Application.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using System.Text.Json; // PagePermissions enum'ının olduğu yer

namespace QrAssignment.Application.Behaviors
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ICurrentUserService _currentUserService;

        // IPermissionService'i kaldırdık çünkü yetkileri artık Token'dan (Claim) okuyoruz.
        // Bu bize inanılmaz bir performans kazandırıyor.
        public AuthorizationBehavior(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Eğer gelen komut/sorgu yetki gerektiriyorsa (ISecuredRequest interface'i varsa)
            if (request is ISecuredRequest securedRequest)
            {
                var userId = _currentUserService.UserId;

                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("Kimlik doğrulama başarısız. Lütfen giriş yapın.");

                // 1. Tüm "permissions" etiketli JSON claimleri çek
                var permissionClaims = _currentUserService.GetClaim("permissions");

                if (string.IsNullOrEmpty(permissionClaims))
                    throw new UnauthorizedAccessException("Sistemde hiçbir yetkiniz bulunmuyor.");

                int totalEffectivePermission = 0;

                // 2. Her bir JSON'u çöz ve ilgili sayfayı bul
                var permissionList = JsonSerializer.Deserialize<List<PermissionDto>>(permissionClaims);
                foreach (var parsedJson in permissionList.Where(w=> w.PageName == securedRequest.PageName))
                {
                    totalEffectivePermission |= parsedJson.PermissionValue;
                }

                // 3. Yetki kontrolü
                if (totalEffectivePermission == 0)
                    throw new UnauthorizedAccessException($"Bu sayfaya ({securedRequest.PageName}) erişim yetkiniz bulunmamaktadır.");

                // ... kalanı aynı (HasFlag vs.)

                // 4. String'den int'e dönen ve birleştirilen sayıyı kendi Flags Enum'ımıza (PagePermissions) cast ediyoruz
                var userPermissions = (PagePermissions)totalEffectivePermission;

                // 5. KONTROL: İstenen spesifik yetki (Örn: ExportExcel) bu toplamın içinde var mı?
                if (!userPermissions.HasFlag(securedRequest.RequiredPermission))
                {
                    throw new UnauthorizedAccessException($"Bu işlemi ({securedRequest.RequiredPermission}) gerçekleştirmek için yetkiniz eksik.");
                }
            }

            return await next();
        }
    }
}