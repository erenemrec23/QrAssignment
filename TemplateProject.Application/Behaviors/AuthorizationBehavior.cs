using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared; // PagePermissions enum'ının olduğu yer
using System;
using System.Threading;
using System.Threading.Tasks;

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

                // 1. O anki kullanıcının token'ından ilgili sayfanın yetki değerini (Örn: "15") okuyoruz
                var userClaimValue = _currentUserService.GetClaim(securedRequest.PageName);

                // 2. Eğer sayfaya ait claim yoksa veya bozuksa (sayıya çevrilemiyorsa), yetkisi yok demektir
                if (string.IsNullOrEmpty(userClaimValue) || !int.TryParse(userClaimValue, out int totalPermissionValue))
                {
                    throw new UnauthorizedAccessException($"Bu sayfaya ({securedRequest.PageName}) erişim yetkiniz bulunmamaktadır.");
                }

                // 3. String'den int'e dönen sayıyı kendi Flags Enum'ımıza (PagePermissions) cast ediyoruz
                var userPermissions = (PagePermissions)totalPermissionValue;

                // 4. KONTROL: İstenen spesifik yetki (Örn: ExportExcel) bu toplamın içinde var mı?
                if (!userPermissions.HasFlag(securedRequest.RequiredPermission))
                {
                    throw new UnauthorizedAccessException($"Bu işlemi ({securedRequest.RequiredPermission}) gerçekleştirmek için yetkiniz eksik.");
                }
            }

            return await next();
        }
    }
}