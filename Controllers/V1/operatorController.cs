using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OperatorController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Test()
        {
            return Ok();
        }
    }
}
