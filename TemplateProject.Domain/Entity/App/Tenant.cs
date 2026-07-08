using QrAssignment.Domain.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;


namespace QrAssignment.Domain.Entity.App
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; }


    }
}

