using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace QrAssignment.Application.Features.Brands.Commands.UpdateBrand
{

    public class UpdateBrandCommand : ICommand<Result>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        
        public byte[] RowVersion { get; set; }


    }
}
