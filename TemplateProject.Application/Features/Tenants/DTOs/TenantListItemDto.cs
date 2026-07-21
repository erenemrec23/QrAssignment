namespace QrAssignment.Application.Features.Tenants.DTOs
{
    public class TenantListItemDto : BaseListItemDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public long RevNum { get; set; }
    }
    public class BaseListItemDto
    {
        public string ModifiedUserFullName { get; set; }
        public string CreatedUserFullName { get; set; }


        public DateTimeOffset? ModifiedDateTime { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
    }
}
 