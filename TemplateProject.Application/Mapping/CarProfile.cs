using AutoMapper;
using QrAssignment.Application.Features.Cars.Commands.CreateCar;
using QrAssignment.Application.Features.Cars.Commands.UpdateCar;
using QrAssignment.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Mapping
{ 
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            // Command -> Entity
            CreateMap<CreateCarCommand, Car>();
            CreateMap<UpdateCarCommand, Car>();

            CreateMap<Car, CreateCarCommand>()
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.Brand.Id));
        }
    }
}
