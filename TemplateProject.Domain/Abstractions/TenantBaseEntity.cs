namespace QrAssignment.Domain.Abstractions
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
    public interface IMustHaveTenant
    {
        Guid? TenantId { get; set; }
    }
    public abstract class TenantBaseEntity : BaseEntity, IMustHaveTenant
    {
         
        public Guid? TenantId { get; set; }
    }
}
