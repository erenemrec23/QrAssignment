using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Entity.System;

namespace QrAssignment.Domain.Entity
{
    public class QrApplicant : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string  LastName { get; set; } 
        public required string Mail { get; set; }

        public string? TCKN { get; set; }
        public virtual SystemRegion? RegionId { get; set; }
    }
}
