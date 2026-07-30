using Microsoft.AspNetCore.Http;
using QrAssignment.Application.Services;

namespace QrAssignment.Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid? _overrideTenantId;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Pre-auth akışlar (ör. şifre sıfırlama) için manuel override.
    // ITenantService scoped olduğu için sadece o request boyunca yaşar.
    public void SetTenantId(Guid tenantId) => _overrideTenantId = tenantId;

    public bool TryGetTenantId(out Guid tenantId)
    {
        if (_overrideTenantId is Guid overridden)
        {
            tenantId = overridden;
            return true;
        }

        var tenantClaim = _httpContextAccessor.HttpContext?.User.FindFirst("TenantId");
        if (tenantClaim is not null && Guid.TryParse(tenantClaim.Value, out tenantId))
            return true;

        tenantId = Guid.Empty;
        return false;
    }

    public Guid GetTenantId()
    {
        if (TryGetTenantId(out var tenantId))
            return tenantId;

        throw new UnauthorizedAccessException("Kullanıcının Tenant (Firma) bilgisi bulunamadı!");
    }
}