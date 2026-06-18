using AutoMapper;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Mapping
{
    public class TenantBulkCreateMappingProfile : Profile
    {
        public TenantBulkCreateMappingProfile()
        {
            CreateMap<CreateTenantInputDto, Tenant>();
        }
    }

}  