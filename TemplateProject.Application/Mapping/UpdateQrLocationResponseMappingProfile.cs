using AutoMapper;
using QrAssignment.Application.Features.QrLocations.Commands.Update;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Application.Mapping
{
    public class UpdateQrLocationResponseMappingProfile : Profile
    {
        public UpdateQrLocationResponseMappingProfile()
        {
            CreateMap<QrLocation, UpdateQrLocationResponse>();

        }
    }


    
}