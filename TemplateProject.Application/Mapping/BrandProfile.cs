using AutoMapper;
using QrAssignment.Application.Features.Brands.Commands.CreateBrand;
using QrAssignment.Application.Features.Brands.Commands.UpdateBrand;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Application.Mapping
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            // Command -> Entity
            CreateMap<CreateBrandCommand, Brand>();
            CreateMap<UpdateBrandCommand, Brand>();

            // Entity -> Response DTO (Örn: Arabaları listelerken marka ismini de almak isterseniz)
            CreateMap<Brand, CreateBrandCommand>();
        }
    }
}
