using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Roles.Commands.BulkDelete;
using QrAssignment.Application.Features.Roles.Commands.Create;
using QrAssignment.Application.Features.Roles.Commands.Delete;
using QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Roles.Commands.Update;
using QrAssignment.Application.Features.Roles.Queries.GetById;
using QrAssignment.Application.Features.Roles.Queries.GetList;
using QrAssignment.Application.Features.Roles.Queries.GetListExportExcel;
using QrAssignment.Application.Features.Roles.Queries.GetPassivedById;
using QrAssignment.Application.Features.Roles.Queries.GetPassivedList; 

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppRolesController : ApiControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateRoleCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new DeleteRoleCommand(id), cancellationToken));

        [HttpPost("export")]
        public async Task<IActionResult> ExportExcel([FromBody] GetListAppRoleExportExcelQuery query, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken);
            if (!result.IsSuccess || result.Value is null)
                return BadRequest(result);

            var file = result.Value;
            return File(file.Data, file.ContentType, file.FileName);
        }

        [HttpGet("sample-export")]
        public async Task<IActionResult> ExportSampleExcel(CancellationToken cancellationToken)
        {
            var query = new GetSampleExcelTemplateQuery<BulkCreateAppRoleInputDto>
            {
                FileName = "role-sample-template.xlsx",
                SampleRowCount = 3
            };

            var result = await Mediator.Send(query, cancellationToken);
            if (!result.IsSuccess || result.Value is null)
                return BadRequest(result);

            var file = result.Value;
            return File(file.Data, file.ContentType, file.FileName);
        }

        [HttpPost("validate-excel")]
        public async Task<IActionResult> ValidateExcel(IFormFile file, CancellationToken cancellationToken)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Geçerli bir dosya yüklenmedi.");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);

            var query = new ValidateExcelQuery<BulkCreateAppRoleInputDto> { FileBytes = memoryStream.ToArray() };
            return HandleResult(await Mediator.Send(query, cancellationToken));
        }

        [HttpPost("bulk-create")]
        public async Task<IActionResult> BulkCreate([FromBody] BulkCreateAppRoleCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        [HttpDelete("Bulk-Delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteAppRoleCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        [HttpGet("Passived/{id}")]
        public async Task<IActionResult> GetPassivedById(Guid? id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetPassivedByIdAppRoleQuery(id), cancellationToken));

        [HttpPost("GetPassivedList")]
        public async Task<IActionResult> GetPassivedList([FromBody] GetPassivedListAppRoleQuery query, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(query, cancellationToken));

        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] GetListAppRoleQuery request, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(request, cancellationToken));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetByIdRoleQuery(id), cancellationToken));
    }
}