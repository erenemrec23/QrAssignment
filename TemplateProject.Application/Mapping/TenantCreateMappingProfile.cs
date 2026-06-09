using AutoMapper;
using QrAssignment.Application.Features.QrLocations.Commands.Create;
using QrAssignment.Application.Features.Tenants.Commands.Create;
using QrAssignment.Application.Features.Tenants.Commands.Update;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Mapping
{
    public class TenantCreateMappingProfile : Profile
    {
        public TenantCreateMappingProfile()
        {
            CreateMap<CreateTenantCommand , Tenant>();
        }
    }
    public class TenantUpdateResponseMappingProfile : Profile
    {
        public TenantUpdateResponseMappingProfile()
        {

            CreateMap<UpdateTenantCommand, Tenant>();
            CreateMap<Tenant, UpdateTenantResponse>();
        }
    }
     
}  