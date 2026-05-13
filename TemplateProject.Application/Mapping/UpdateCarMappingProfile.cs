using AutoMapper;
using TemplateProject.Application.Features.Cars.Commands.UpdateCar;
using TemplateProject.Domain.Entity;

namespace TemplateProject.Application.Mapping
{
    public class UpdateCarMappingProfile : Profile
    {
        public UpdateCarMappingProfile()
        {

            CreateMap<UpdateCarCommand, Car>().ReverseMap();
            CreateMap<Car, UpdatedCarResponse>().ReverseMap();
        }
    }
}
