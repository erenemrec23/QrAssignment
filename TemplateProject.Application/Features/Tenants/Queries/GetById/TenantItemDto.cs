using QrAssignment.Application.Features.Tenants.Queries.GetList;

namespace QrAssignment.Application.Features.Tenants.Queries.GetById
{
    public class TenantItemDto : TenantListItemDto
    {
        public byte[] RowVersion { get; set; }
    }
}
 