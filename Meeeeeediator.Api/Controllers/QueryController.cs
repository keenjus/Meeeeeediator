using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Meeeeeediator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly ILogger<QueryController> _logger;

        public QueryController(ILogger<QueryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(new { success = true });
        }

        [HttpPost]
        public IActionResult Query()
        {
            return new JsonResult(new { success = true });
        }
    }
}
