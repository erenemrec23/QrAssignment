using Microsoft.AspNetCore.Mvc; 

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ModulesController : ApiControllerBase
    {

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSystemModules(CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetSystemModulesQuery(), cancellationToken));
    }
}