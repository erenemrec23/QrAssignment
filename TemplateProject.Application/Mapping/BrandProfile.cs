using AutoMapper;
using TemplateProject.Application.Features.Brands.Commands.CreateBrand;
using TemplateProject.Application.Features.Brands.Commands.UpdateBrand;
using TemplateProject.Domain.Entity;

namespace TemplateProject.Application.Mapping
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
