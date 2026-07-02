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
         
        public AuthorizationBehavior(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ISecuredRequest securedRequest)
            {
                var userId = _currentUserService.UserId;

                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("Kimlik doğrulama başarısız. Lütfen giriş yapın.");
                 
                var permissionClaims = _currentUserService.GetClaim("permissions");

                if (string.IsNullOrEmpty(permissionClaims))
                    throw new UnauthorizedAccessException("Sistemde hiçbir yetkiniz bulunmuyor.");

                int totalEffectivePermission = 0;
                 
                var permissionList = JsonSerializer.Deserialize<List<PermissionDto>>(permissionClaims);
                foreach (var parsedJson in permissionList.Where(w=> w.PageName == securedRequest.PageName))
                {
                    totalEffectivePermission |= parsedJson.PermissionValue;
                }
                 
                if (totalEffectivePermission == 0)
                    throw new UnauthorizedAccessException($"Bu sayfaya ({securedRequest.PageName}) erişim yetkiniz bulunmamaktadır.");
                 
                 
                var userPermissions = (PagePermissions)totalEffectivePermission;
                 
                if (!userPermissions.HasFlag(securedRequest.RequiredPermission))
                {
                    throw new UnauthorizedAccessException($"Bu işlemi ({securedRequest.RequiredPermission}) gerçekleştirmek için yetkiniz eksik.");
                }
            }

            return await next();
        }
    }
}