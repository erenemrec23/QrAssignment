using AutoMapper;
using QrAssignment.Application.Features.BrandWithCar.Commands;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Application.Mapping
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
