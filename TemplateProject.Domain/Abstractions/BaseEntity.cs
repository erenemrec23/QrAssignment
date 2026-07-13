using QrAssignment.Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace QrAssignment.Domain.Abstractions
{

    public interface IBaseEntity
    {
        Guid Id { get; set; }

        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset? ModifiedDate { get; set; }

        Guid? CreatedByUserId { get; set; }
        Guid? ModifiedByUserId { get; set; }

        long RevNum { get; set; }
        bool IsDeleted { get; set; }


        [Timestamp]
        byte[] RowVersion { get; set; }
    }
    public class BaseEntity : IBaseEntity
    { 
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid? CreatedByUserId { get; set; }

        [Filterable]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Filterable]
        public DateTimeOffset? ModifiedDate { get; set; }
        public Guid? ModifiedByUserId { get; set; }
        public bool IsDeleted { get; set; }

        [Timestamp] 
        public byte[] RowVersion { get; set; } = null!;

        [Filterable]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RevNum { get; set; }
    }


  
}
