using AutoMapper; 
using QrAssignment.Application.Features.QrLocations.Commands.Create; 
using QrAssignment.Domain.Entity;

namespace QrAssignment.Application.Mapping
{
    public class QrLocationCreateMappingProfile : Profile
    {
        public QrLocationCreateMappingProfile()
        { 
            CreateMap<QrLocation, CreateQrLocationCommand>()
                .ForMember(dest => dest.ParentLocationId, opt =>
                    opt.MapFrom(src => src.ParentLocation != null ? src.ParentLocation.Id : (Guid?)null));
             
            CreateMap<CreateQrLocationCommand, QrLocation>() 
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.ParentLocation, opt => opt.Ignore())
                .ForMember(dest => dest.SubLocations, opt => opt.Ignore()); 
        }
    }
    
}