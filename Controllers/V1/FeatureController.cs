using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Feature;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeatureController(IFeatureService featureService) : ControllerBase
    {
        [HttpGet("list")]
        public async Task<ActionResult<ResponseDto<IEnumerable<FeatureDto>>>> GetFeatureListAsync()
        {
            var res = await featureService.GetFeatureListAsync();
            return Ok(res);
        }

        [HttpGet("role/{RoleId}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<FeatureDto>>>> GetFeatureByRoleIdAsync(short RoleId)
        {
            var res = await featureService.GetFeatureByRoleAsync(RoleId);
            return Ok(res);
        }

        [HttpGet("role/{RoleId}/{FeatureId}")]
        public async Task<ActionResult<ResponseDto<FeatureDto>>> GetOneFeatureByRoleIdAsync(short RoleId,short FeatureId)
        {
            var res = await featureService.GetOneFeatureByRoleIdAsync(RoleId,FeatureId);
            return Ok(res);
        }
    }
}
