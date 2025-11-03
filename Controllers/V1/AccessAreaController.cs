using HIDAeroService.DTO;
using HIDAeroService.DTO.AccessArea;
using HIDAeroService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace HIDAeroService.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessAreaController(IAccessAreaService accessareaService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<AccessAreaDto>>>> GetAsync()
        {
            var res = await accessareaService.GetAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<bool>>> Create([FromBody] AccessAreaDto dto) 
        {
            var res = await accessareaService.CreateAsync(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto<AccessAreaDto>>> Update([FromBody] AccessAreaDto dto)
        {
            var res = await accessareaService.UpdateAsync(dto);
            return Ok(res);
        }

        [HttpDelete("{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<bool>>> Delete(string mac,short component)
        {
            var res = await accessareaService.DeleteAsync(mac,component);
            return Ok(res);
        }

        [HttpGet("{mac}/{component}")]
        public async Task<ActionResult<ResponseDto<AccessAreaDto>>> GetByComponentAsync(string mac,short component)
        {
            var res = await accessareaService.GetByComponentAsync(mac,component);
            return Ok(res);
        }


    }
}
