using System.ComponentModel.DataAnnotations;
using TemplateProject.Application.Abstractions;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.Brands.Commands.UpdateBrand
{

    public class UpdateBrandCommand : ICommand<Result>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        
        public byte[] RowVersion { get; set; }


    }
}
