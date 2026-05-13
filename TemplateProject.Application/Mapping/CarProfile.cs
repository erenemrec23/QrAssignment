using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Application.Features.Cars.Commands.CreateCar;
using TemplateProject.Application.Features.Cars.Commands.UpdateCar;
using TemplateProject.Domain.Entity;

namespace TemplateProject.Application.Mapping
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
