using Meeeeeediator.Api.Queries;
using Meeeeeediator.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Meeeeeediator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly ILogger<QueryController> _logger;
        private readonly IMediator _mediator;

        public QueryController(ILogger<QueryController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Query(string name, [FromBody]string rawQuery)
        {
            var result = await _mediator.SendAsync(name, rawQuery);

            return new JsonResult(new { data = result });
        }
    }
}
