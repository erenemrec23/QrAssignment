using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace QrAssignment.Domain.Abstractions
{

    public interface IBaseEntity
    {
        Guid Id { get; set; }

        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset? ModifiedDate { get; set; }

        bool IsDeleted { get; set; }


        [Timestamp]
        byte[] RowVersion { get; set; }
    }
    public class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid? CreatedByUserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedDate { get; set; }
        public Guid? ModifiedByUserId { get; set; }
        public bool IsDeleted { get; set; }

        [Timestamp] 
        public byte[] RowVersion { get; set; } = null!;

        public long RevNum { get; set; }
    }


  
}
