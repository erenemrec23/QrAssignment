using ExcelDataReader;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Tenants.Commands.Create;
using QrAssignment.Application.Features.Tenants.Commands.Delete;
using QrAssignment.Application.Features.Tenants.Commands.BulkDelete;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Tenants.Commands.Excel.Validate;
using QrAssignment.Application.Features.Tenants.Commands.Update;
using QrAssignment.Application.Features.Tenants.Queries.GetById;
using QrAssignment.Application.Features.Tenants.Queries.GetList;
using QrAssignment.Application.Features.Tenants.Queries.GetListExportExcel;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TenantsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateTenantCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateTenantCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            // 1. Gelen ID'yi Command nesnemizin içine koyuyoruz
            var command = new DeleteTenantCommand { Id = id };

            // 2. MediatR'a komutu gönderip Handler'ın çalışmasını bekliyoruz
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPost("bulk-delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteTenantCommand command, CancellationToken cancellationToken)
        {
            // Command nesnesini handler'a gönderiyoruz
            Result result = await _mediator.Send(command, cancellationToken);

            // Result nesnesinin başarılı olup olmadığını kontrol ederek uygun HTTP durum kodunu dönüyoruz
            if (result.IsFailure)
            {
                return BadRequest(result.Error); // Projenizdeki Result yapısına göre result.ToProblemDetails() vb. de kullanabilirsiniz
            }

            return Ok(result); // Vear NoContent();
        }

        [HttpPost("GetList")]  
        public async Task<IActionResult> GetList([FromBody] GetListTenantQuery request) // 2. FromQuery yerine FromBody yapıyoruz
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetByIdTenantQuery(id);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportExcel([FromBody] GetTenantListExportExcelQuery query, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess || result.Value == null)
            {
                return BadRequest(result.Error);
            }

            var fileDto = result.Value;

            // Dosyayı Blob formatında istemciye fırlat
            return File(fileDto.Data, fileDto.ContentType, fileDto.FileName);
        }

        [HttpPost("validate-excel")]
        public async Task<IActionResult> ValidateExcel(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Geçerli bir dosya yüklenmedi.");

            // Dosyayı byte array'e çıkarıyoruz (Application katmanını HTTP bağımlılığından kurtarmak için)
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);

            var query = new ValidateTenantExcelQuery
            {
                FileBytes = memoryStream.ToArray()
            };

            var result = await _mediator.Send(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result); // Angular tarafına ExcelValidationResponseDto döner

            return BadRequest(result);
        }
        // 2. ADIM: Önizlemeden onay alan temiz datayı kaydet
        [HttpPost("bulk-create")]
        public async Task<IActionResult> BulkCreate([FromBody] BulkCreateTenantCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }
    }
}
