using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Application.Features.BrandWithCar.Commands;
using TemplateProject.Application.Features.Cars.Commands.CreateCar;
using TemplateProject.Application.Features.Cars.Commands.UpdateCar;
using TemplateProject.Application.Features.Cars.Queries.GetList;

namespace TemplateProject.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    //[Authorize]
    public class CarsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CarsController(IMediator mediator) => _mediator = mediator;
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateCarCommand command)
        {
            // MediatR'a komutu veriyoruz. 
            // O arka planda gidip uygun Handler'ı (CreateCarCommandHandler) bulup çalıştırıyor.
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateWithBrand(CreateBrandWithCarCommand command)
        { 
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateCar(UpdateCarCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetList([FromQuery] GetListCarQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

    }
}
