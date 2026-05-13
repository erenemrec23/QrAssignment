using AutoMapper;
using TemplateProject.Application.Features.BrandWithCar.Commands;
using TemplateProject.Domain.Entity;

namespace TemplateProject.Application.Mapping
{
    public class BrandMappingProfile : Profile
    {
        public BrandMappingProfile()
        {
            CreateMap<CreateBrandWithCarCommand, Brand>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.BrandName));

            // Request -> Car eşlemesi
            CreateMap<CreateBrandWithCarCommand, Car>()
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.CarModel))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.CarYear));
        }
    }
}
