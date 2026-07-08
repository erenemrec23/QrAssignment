using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Attributes;

namespace QrAssignment.Domain.Entity
{
    public class QrLocation : TenantBaseEntity
    {
        [Filterable]
        public required string Name { get; set; }

        [Filterable]
        public DateTimeOffset? StartDate { get; set; }

        [Filterable]
        public DateTimeOffset? EndDate { get; set; }

        [Filterable]
        public string? LocationName { get; set; }

        public Guid? ParentLocationId { get; set; }

        public virtual QrLocation? ParentLocation { get; set; }

        public virtual ICollection<QrLocation> SubLocations { get; set; } = new List<QrLocation>();
    }

}
