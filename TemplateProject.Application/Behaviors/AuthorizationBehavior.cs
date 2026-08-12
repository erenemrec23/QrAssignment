using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.DTOs;
using QrAssignment.Application.Security;
using System.Text.Json;
using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Application.Behaviors
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppLocalizer _localizer;

        public AuthorizationBehavior(ICurrentUserService currentUserService, IAppLocalizer localizer)
        {
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestType = request.GetType();

            // 1. KONTROL: Bu komut serbest listesinde mi? (login gerekmez)
            if (AuthorizationRegistry.UnsecuredCommands.Contains(requestType))
            {
                return await next();
            }

            // 2. KONTROL: Login şart ama permission kontrolü yok
            if (AuthorizationRegistry.AuthenticatedOnlyCommands.Contains(requestType))
            {
                var authOnlyUserId = _currentUserService.UserId;
                if (string.IsNullOrEmpty(authOnlyUserId))
                    throw new UnauthorizedAccessException(_localizer["Authorization.NotAuthenticated"]);

                return await next();
            }

            // 3. KONTROL: Bu komut güvenli listede mi?
            if (AuthorizationRegistry.SecuredCommands.TryGetValue(requestType, out var authRequirement))
            {
                var userId = _currentUserService.UserId;
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException(_localizer["Authorization.NotAuthenticated"]);

                // PageName registry'de sabitse onu kullan; null ise (dynamic) request'ten oku
                string pageKey = authRequirement.PageName
                    ?? ResolveDynamicPageKey(request, requestType);

                var permissionsClaimValue = _currentUserService.GetClaims("permissions").FirstOrDefault();
                if (string.IsNullOrEmpty(permissionsClaimValue))
                    throw new UnauthorizedAccessException(_localizer["Authorization.NoPermissions"]);

                try
                {
                    var permissionsList = JsonSerializer.Deserialize<List<PermissionDto>>(permissionsClaimValue);
                    if (permissionsList == null || !permissionsList.Any())
                        throw new UnauthorizedAccessException(_localizer["Authorization.NoPermissions"]);

                    int totalEffectivePermission = 0;

                    foreach (var permission in permissionsList)
                    {
                        if (permission.PageName == pageKey)
                        {
                            totalEffectivePermission |= permission.PermissionValue;
                        }
                    }

                    if (totalEffectivePermission == 0)
                        throw new UnauthorizedAccessException(string.Format(_localizer["Authorization.PageAccessDenied"], pageKey));

                    var userPermissions = (PageAccessFlags)totalEffectivePermission;

                    if (!userPermissions.HasFlag(authRequirement.Permission))
                        throw new UnauthorizedAccessException(_localizer["Authorization.InsufficientPermission"]);
                }
                catch (JsonException)
                {
                    throw new UnauthorizedAccessException(_localizer["Authorization.InvalidPermissionFormat"]);
                }

                return await next();
            }

            throw new InvalidOperationException($"Güvenlik İhlali: '{requestType.Name}' komutu AuthorizationRegistry içerisinde Secured veya Unsecured olarak tanımlanmamış!");
        }

        /// <summary>
        /// Registry'de PageName null ise (dynamic/page-scoped komut), request IPageScopedRequest
        /// implement etmek zorundadır. Etmiyorsa bu bir konfigürasyon hatasıdır, sessizce geçilmez.
        /// </summary>
        private string ResolveDynamicPageKey(TRequest request, Type requestType)
        {
            if (request is not IPageScopedRequest pageScoped)
                throw new InvalidOperationException(
                    $"Güvenlik İhlali: '{requestType.Name}' registry'de dynamic (PageName=null) olarak kayıtlı ama IPageScopedRequest implement etmiyor.");

            if (string.IsNullOrWhiteSpace(pageScoped.PageKey))
                throw new UnauthorizedAccessException(_localizer["Authorization.InvalidPageKey"]);

            return pageScoped.PageKey;
        }
    }
}