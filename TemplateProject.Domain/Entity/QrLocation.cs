using QrAssignment.Domain.Abstractions;

namespace QrAssignment.Domain.Entity
{
    public class QrLocation : BaseEntity
    {
        public required string Name { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public string? LocationName { get; set; }

        public Guid? ParentLocationId { get; set; }

        public virtual QrLocation? ParentLocation { get; set; }

        public virtual ICollection<QrLocation> SubLocations { get; set; } = new List<QrLocation>();
    }
}
