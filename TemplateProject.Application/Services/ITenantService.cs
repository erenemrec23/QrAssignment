namespace QrAssignment.Application.Services
{
    public interface ITenantService
    {
        Guid GetTenantId();
        bool TryGetTenantId(out Guid tenantId);
        void SetTenantId(Guid tenantId);
    }
}