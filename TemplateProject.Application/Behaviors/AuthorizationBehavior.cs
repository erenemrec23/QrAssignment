using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using QrAssignment.Application.DTOs;
using QrAssignment.Application.Security; // Registry'nin olduğu namespace
using System.Text.Json;

namespace QrAssignment.Application.Behaviors
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ICurrentUserService _currentUserService;

        public AuthorizationBehavior(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestType = request.GetType();

            // 1. KONTROL: Bu komut serbest listesinde mi?
            if (AuthorizationRegistry.UnsecuredCommands.Contains(requestType))
            {
                // Hiçbir güvenlik kontrolü yapmadan işleme devam et
                return await next();
            }

            // 2. KONTROL: Bu komut güvenli listede mi?
            if (AuthorizationRegistry.SecuredCommands.TryGetValue(requestType, out var authRequirement))
            {
                var userId = _currentUserService.UserId;
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("Kimlik doğrulama başarısız. Lütfen giriş yapın.");

                var permissionsClaimValue = _currentUserService.GetClaims("permissions").FirstOrDefault();
                if (string.IsNullOrEmpty(permissionsClaimValue))
                    throw new UnauthorizedAccessException("Sistemde hiçbir yetkiniz bulunmuyor.");

                try
                {
                    var permissionsList = JsonSerializer.Deserialize<List<PermissionDto>>(permissionsClaimValue);
                    if (permissionsList == null || !permissionsList.Any())
                        throw new UnauthorizedAccessException("Yetki listesi boş veya çözümlenemedi.");

                    int totalEffectivePermission = 0;

                    // Kayıt defterinden gelen sayfa adına (authRequirement.PageName) göre filtrele
                    foreach (var permission in permissionsList)
                    {
                        if (permission.PageName == authRequirement.PageName)
                        {
                            totalEffectivePermission |= permission.PermissionValue;
                        }
                    }

                    if (totalEffectivePermission == 0)
                        throw new UnauthorizedAccessException($"Bu sayfaya ({authRequirement.PageName}) erişim yetkiniz bulunmamaktadır.");

                    var userPermissions = (PagePermissions)totalEffectivePermission;

                    // Kayıt defterinden gelen yetki seviyesine (authRequirement.Permission) göre kontrol et
                    if (!userPermissions.HasFlag(authRequirement.Permission))
                        throw new UnauthorizedAccessException($"Bu işlemi gerçekleştirmek için yetkiniz eksik.");
                }
                catch (JsonException)
                {
                    throw new UnauthorizedAccessException("Yetki verisi formatı hatalı.");
                }

                // Yetki tamamsa işleme devam et
                return await next();
            }

            // 3. KONTROL: Komut defterde YANLIŞLIKLA UNUTULMUŞ! (Güvenlik Açığı Koruması)
            // Sistemdeki bir Command bu iki listeden birinde değilse, uygulamanın çalışmasını kasıtlı olarak durduruyoruz.
            throw new InvalidOperationException($"Güvenlik İhlali: '{requestType.Name}' komutu AuthorizationRegistry içerisinde Secured veya Unsecured olarak tanımlanmamış!");
        }
    }
}