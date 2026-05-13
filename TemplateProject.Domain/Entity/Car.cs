using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Domain.Abstractions;

namespace TemplateProject.Domain.Entity
{
    public class Car : BaseEntity
    { 
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public string? Model { get; set; }

        public int Year { get; set; }
    }
}
