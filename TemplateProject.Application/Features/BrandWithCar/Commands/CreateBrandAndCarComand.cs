using MediatR;
using TemplateProject.Application.Abstractions;
using TemplateProject.Domain.Shared;

namespace TemplateProject.Application.Features.BrandWithCar.Commands
{
    public class CreateBrandWithCarCommand : ICommand<Result<Guid>>
    {
        public string BrandName { get; set; } // Marka adı
        public string CarModel { get; set; }   // Araba modeli
        public int CarYear { get; set; }       // Araba yılı
    }
}
