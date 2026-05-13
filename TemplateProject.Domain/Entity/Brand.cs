using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Domain.Abstractions;

namespace TemplateProject.Domain.Entity
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
