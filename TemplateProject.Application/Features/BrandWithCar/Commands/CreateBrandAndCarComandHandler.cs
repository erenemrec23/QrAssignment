using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.BrandWithCar.Commands
{
    public class CreateBrandWithCarCommandHandler : IRequestHandler<CreateBrandWithCarCommand, Result<Guid>>
    { 
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;
        private readonly ICarRepository _carRepository;
        public CreateBrandWithCarCommandHandler(IMapper mapper, IBrandRepository brandRepository, ICarRepository carRepository)
        { 
            _mapper = mapper;
            _brandRepository = brandRepository;
            _carRepository = carRepository;
        }

        public async Task<Result<Guid>> Handle(CreateBrandWithCarCommand request, CancellationToken cancellationToken)
        {
            var brand = _mapper.Map<Brand>(request);
            var car = _mapper.Map<Car>(request); 
            car.Brand = brand; 
            await _brandRepository.AddAsync(brand, cancellationToken);
            await _carRepository.AddAsync(car, cancellationToken);


            return Result.Success(brand.Id);
        }
    }
}
