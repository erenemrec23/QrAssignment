using AutoMapper;
using QrAssignment.Application.Features.Cars.Commands.UpdateCar;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Application.Mapping
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
