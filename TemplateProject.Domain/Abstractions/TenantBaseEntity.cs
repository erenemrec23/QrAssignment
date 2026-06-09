namespace QrAssignment.Domain.Abstractions
{
    public interface IMustHaveTenant
    {
        Guid? TenantId { get; set; }
    }
    public abstract class TenantBaseEntity : BaseEntity, IMustHaveTenant
    {
         
        public Guid? TenantId { get; set; }
    }
}
