using Meeeeeediator.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Meeeeeediator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QueryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Query(string name, [FromBody]string rawQuery)
        {
            return new JsonResult(new { data = await _mediator.SendAsync(name, rawQuery) });
        }
    }
}
