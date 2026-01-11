using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Feature;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeatureController(IFeatureService service) : ControllerBase
    {
        [HttpGet("list")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<FeatureDto>>>> GetFeatureListAsync()
        {
            var res = await service.GetFeatureListAsync();
            return Ok(res);
        }

        [HttpGet("role/{roleid}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<FeatureDto>>>> GetFeatureByRoleIdAsync(short roleid)
        {
            var res = await service.GetFeatureByRoleAsync(roleid);
            return Ok(res);
        }

        [HttpGet("role/{roleid}/{featureid}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<FeatureDto>>> GetOneFeatureByRoleIdAsync(short roleid, short featureid)
        {
            var res = await service.GetOneFeatureByRoleIdAsync(roleid, featureid);
            return Ok(res);
        }
    }
}
