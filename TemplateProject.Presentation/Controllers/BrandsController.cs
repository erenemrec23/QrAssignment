using MediatR;
using Microsoft.AspNetCore.Mvc;
using TemplateProject.Application.Features.Brands.Commands.CreateBrand;
using TemplateProject.Application.Features.Brands.Commands.UpdateBrand;

namespace TemplateProject.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BrandsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateBrandCommand command)
        {
            // MediatR'a komutu veriyoruz. 
            // O arka planda gidip uygun Handler'ı (CreateCarCommandHandler) bulup çalıştırıyor.
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Update(UpdateBrandCommand command)
        {
            // MediatR'a komutu veriyoruz. 
            // O arka planda gidip uygun Handler'ı (CreateCarCommandHandler) bulup çalıştırıyor.
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
