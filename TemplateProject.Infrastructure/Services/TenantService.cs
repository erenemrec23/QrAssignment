using Microsoft.AspNetCore.Http;
using QrAssignment.Application.Services;

namespace QrAssignment.Infrastructure.Services; 

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetTenantId()
    {
        //return Guid.Parse("019EABF9-40CA-7C3D-9901-C8A1F007771E");
        var tenantClaim = _httpContextAccessor.HttpContext?.User.FindFirst("TenantId");

        if (tenantClaim == null)
            throw new UnauthorizedAccessException("Kullanıcının Tenant (Firma) bilgisi bulunamadı!");

        return Guid.Parse(tenantClaim.Value);
    }
}