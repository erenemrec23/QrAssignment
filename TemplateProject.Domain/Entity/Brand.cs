using QrAssignment.Domain.Abstractions;

namespace QrAssignment.Domain.Entity
{
    public class Brand : BaseEntity
    { 
        public string Name { get; set; }

        // İlişki: Bir markanın birden fazla arabası olabilir.
        public ICollection<Car> Cars { get; set; }

        public Brand()
        { 
            Cars = new HashSet<Car>();
        }
    }
}
