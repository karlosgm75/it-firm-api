
using Microsoft.AspNetCore.Mvc;

namespace it_firm_api.Controllers
{
    [Route("")]
    [ApiController]
    public class HealthController : ControllerBase {
        [HttpGet]
        public IActionResult Get() { 
        return Ok();
        }
    }
}
