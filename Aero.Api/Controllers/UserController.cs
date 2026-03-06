using Aero.Api.Models;
using Aero.Application.DTOs;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Aero.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService cardHolderService,IFileStorage file) : ControllerBase
    {


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<UserDto>>>> GetAsync()
        {
            var res = await cardHolderService.GetAsync();
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<IEnumerable<UserDto>>>> GetByLocationIdAsync(short location)
        {
            var res = await cardHolderService.GetByLocationIdAsync(location);
            return Ok(res);
        }

        [HttpGet("/api/{location}/[controller]/pagination")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<Pagination<UserDto>>>> GetPaginatinAsync( [FromQuery] PaginationParamsWithFilter param,short location)
        {
            var res = await cardHolderService.GetPaginationAsync(param,location);
            return Ok(res);
        }

        [HttpGet("{userid}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<UserDto>>> GetByComponentAsync(string userid)
        {
            var res = await cardHolderService.GetByUserIdAsync(userid);
            return Ok(res);
        }

        [HttpGet("image/{userid}")]
        [Authorize]
        [Produces("image/png")]
        public async Task<IActionResult> GetImageAsync(string userid)
        {
            if (string.IsNullOrEmpty(userid)) return BadRequest();
            var stream = await file.ReadUserAsync(userid);

            return File(stream, "image/png");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> CreateAsync([FromBody] UserDto dto)
        {

            var res = await cardHolderService.CreateAsync(dto);
            return Ok(res);
        }


        [Authorize]
        [HttpPost("upload/{userid}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ResponseDto<bool>>> UploadImageAsync([FromForm] UploadImageRequest request,string userid)
        {
            var res = await cardHolderService.UploadImageAsync(userid, request.Image.OpenReadStream());
            return Ok(res);
        }


        [HttpDelete("image/{userid}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<bool>>> DeleteImageAsync(string userid) 
        {
            var res = await cardHolderService.DeleteImageAsync(userid);
            return Ok(res);
        }


        [HttpDelete("{userid}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<UserDto>>> DeleteAsync(string userid)
        {
            var res = await cardHolderService.DeleteAsync(userid);
            return Ok(res);
        }

        

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseDto<UserDto>>> UpdateAsync([FromBody] UserDto dto)
        {
            var res = await cardHolderService.UpdateAsync(dto);
            return Ok(res);
        }
    }
}
