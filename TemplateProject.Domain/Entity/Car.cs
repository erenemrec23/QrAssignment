using QrAssignment.Domain.Abstractions;

namespace QrAssignment.Domain.Entity
{
    public class Car : BaseEntity
    { 
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public string? Model { get; set; }

        public int Year { get; set; }
    }
}
