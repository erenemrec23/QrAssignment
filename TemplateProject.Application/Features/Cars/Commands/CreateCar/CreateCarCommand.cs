using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Cars.Commands.CreateCar;
 
public class CreateCarCommand : ICommand<Result<Guid>>
{
    public Guid BrandId { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }


}
