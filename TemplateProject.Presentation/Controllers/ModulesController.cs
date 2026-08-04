using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Menu.Queries.GetList;
namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ModulesController : ApiControllerBase
    {

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSystemModules(CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetSystemModulesQuery(), cancellationToken));

        [HttpGet("GetMenu")]
        public async Task<IActionResult> GetMenu(CancellationToken ct)
    => HandleResult(await Mediator.Send(new GetListMenuQuery(), ct));
    }
}