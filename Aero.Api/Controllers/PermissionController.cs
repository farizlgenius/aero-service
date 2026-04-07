using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionController(IPermissionService service) : ControllerBase
    {
        [HttpGet("list")]
        public async Task<ActionResult<ResponseDto<IEnumerable<PermissionDto>>>> GetFeatureListAsync()
        {
            var res = await service.GetFeatureListAsync();
            return Ok(res);
        }

        [HttpGet("role/{roleid}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<PermissionDto>>>> GetFeatureByRoleIdAsync(short roleid)
        {
            var res = await service.GetFeatureByRoleAsync(roleid);
            return Ok(res);
        }

        [HttpGet("role/{roleid}/{featureid}")]
        public async Task<ActionResult<ResponseDto<PermissionDto>>> GetOneFeatureByRoleIdAsync(short roleid, short featureid)
        {
            var res = await service.GetFeatureByRoleIdAndFeatureIdAsync(roleid, featureid);
            return Ok(res);
        }
    }
}
