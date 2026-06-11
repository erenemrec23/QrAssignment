namespace QrAssignment.Application.Features.Tenants.Queries.GetList
{
    public class TenantListItemDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }

    public class TenantItemDto : TenantListItemDto
    {
        public byte[] RowVersion { get; set; }
    }
}
