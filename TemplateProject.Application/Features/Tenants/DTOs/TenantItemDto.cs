
namespace QrAssignment.Application.Features.Tenants.DTOs
{
    public class TenantItemDto : TenantListItemDto
    {
        public byte[] RowVersion { get; set; }
    }
}
 