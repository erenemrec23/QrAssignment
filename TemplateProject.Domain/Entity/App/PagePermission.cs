using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Domain.Entity.App
{
    public sealed class PagePermission : IMustHaveTenant
    {
        public Guid Id { get; set; }

        // Sahip: ikisinden TAM BİRİ dolu (CHECK constraint garanti eder)
        public Guid? UserId { get; set; }
        public AppUser? User { get; set; }

        public Guid? RoleId { get; set; }
        public AppRole? Role { get; set; }

        public int PageId { get; set; }                       // = (int)AppPage
        public Page Page { get; set; } = null!;

        public PagePermissions PermissionValue { get; set; }  // [Flags] enum → int kolon

        public Guid? TenantId { get; set; }                   // sahibinden kopyalanır

        public static PagePermission ForUser(Guid userId, int pageId, PagePermissions value, Guid? tenantId)
            => new() { UserId = userId, PageId = pageId, PermissionValue = value, TenantId = tenantId };

        public static PagePermission ForRole(Guid roleId, int pageId, PagePermissions value, Guid? tenantId)
            => new() { RoleId = roleId, PageId = pageId, PermissionValue = value, TenantId = tenantId };
    }
}