using TemplateProject.Application.Abstractions;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.Brands.Commands.CreateBrand;

public class CreateBrandCommand   : ICommand<Result<Guid>>
{
    public string Name { get; set; } 


}

