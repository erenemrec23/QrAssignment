using AutoMapper;
using MediatR;
using TemplateProject.Application.Abstractions;
using TemplateProject.Application.Repositories;
using TemplateProject.Domain.Entity;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.Cars.Commands.CreateCar;

public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, Result<Guid>>
{ 
    private readonly IMapper _mapper; 
    private readonly ICarRepository _carRepository;
    public CreateCarCommandHandler(ICarRepository carRepository ,IMapper mapper)
    { 
        _mapper = mapper;
        _carRepository = carRepository; 
    }

    public async Task<Result<Guid>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    { 

        var car = _mapper.Map<Car>(request);
        await _carRepository.AddAsync(car, cancellationToken); 
        return Result.Success(car.Id);
    }
}
