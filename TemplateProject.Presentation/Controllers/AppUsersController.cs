using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Users.Commands.BulkDelete;
using QrAssignment.Application.Features.Users.Commands.BulkSetPassive;
using QrAssignment.Application.Features.Users.Commands.Create;
using QrAssignment.Application.Features.Users.Commands.Delete;
using QrAssignment.Application.Features.Users.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Users.Commands.Excel.Validate; 
using QrAssignment.Application.Features.Users.Commands.SetActive;
using QrAssignment.Application.Features.Users.Commands.SetPassive;
using QrAssignment.Application.Features.Users.Commands.Update;
using QrAssignment.Application.Features.Users.Queries.FormBase.GetById;
using QrAssignment.Application.Features.Users.Queries.FormBase.GetPassivedById;
using QrAssignment.Application.Features.Users.Queries.ListBase.GetList;
using QrAssignment.Application.Features.Users.Queries.ListBase.GetListExportExcel;
using QrAssignment.Application.Features.Users.Queries.ListBase.GetPassivedList;
using QrAssignment.Application.Features.Users.Queries.LookUp.GetLookupList;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUsersController : ApiControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateAppUserCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateAppUserCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken)); 
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] GetListAppUserQuery query, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(query, cancellationToken));

        [HttpPost("GetPassivedList")]
        public async Task<IActionResult> GetPassivedList([FromBody] GetPassivedListAppUserQuery query, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(query, cancellationToken));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetLookUpList(CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetLookUpListAppUserQuery(), cancellationToken));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetByIdAppUserQuery(id), cancellationToken));

        [HttpGet("Passived/{id:guid}")]
        public async Task<IActionResult> GetPassivedById([FromRoute] Guid? id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetPassivedByIdAppUserQuery(id), cancellationToken));

        // --- EKSİK OLAN EXCEL VE BULK ENDPOINT'LERİ ---

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromBody] GetListAppUserExportExcelQuery query, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(query, cancellationToken));

        [HttpPost("ValidateExcel")]
        public async Task<IActionResult> ValidateExcel([FromBody] ValidateAppUserExcelQuery query, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(query, cancellationToken));

        [HttpPost("BulkCreate")]
        public async Task<IActionResult> BulkCreate([FromBody] BulkCreateAppUserCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        [HttpGet("GetSampleExcelTemplate")]
        public async Task<IActionResult> GetSampleExcelTemplate(CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetSampleExcelTemplateQuery<BulkCreateAppUserInputDto>(), cancellationToken));

        [HttpPut("SetPassive/{id:guid}")]
        public async Task<IActionResult> SetPassive([FromRoute] Guid id, CancellationToken cancellationToken)
    => HandleResult(await Mediator.Send(new SetPassiveAppUserCommand(id), cancellationToken));


        [HttpPut("SetActive/{id:guid}")]
        public async Task<IActionResult> SetActive([FromRoute] Guid id, CancellationToken cancellationToken)
    => HandleResult(await Mediator.Send(new SetActiveAppUserCommand(id), cancellationToken));


        [HttpDelete("Bulk-Delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteAppUserCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpDelete("Bulk-SetActive")]
        public async Task<IActionResult> BulkSetActive([FromBody] BulkSetActiveAppUserCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpDelete("Bulk-SetPassive")]
        public async Task<IActionResult> BulkSetPassive([FromBody] BulkSetPassiveAppUserCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new DeleteAppUserCommand(id), cancellationToken));
    }
}