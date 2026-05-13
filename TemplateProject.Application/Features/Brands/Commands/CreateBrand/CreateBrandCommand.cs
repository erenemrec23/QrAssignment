using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Brands.Commands.CreateBrand;

public class CreateBrandCommand   : ICommand<Result<Guid>>
{
    public string Name { get; set; } 


}

