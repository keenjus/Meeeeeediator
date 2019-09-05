using Meeeeeediator.Api.Queries;
using Meeeeeediator.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<IActionResult> Query()
        {
            var result = await _mediator.SendAsync(new EchoQuery() { Message = "TEST" });

            return new JsonResult(new { data = result });
        }
    }
}
