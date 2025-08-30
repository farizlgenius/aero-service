using HIDAeroService.Dto;
using HIDAeroService.Dto.AccessLevel;
using HIDAeroService.Entity;
using HIDAeroService.Service.Impl;
using HIDAeroService.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using HIDAeroService.Helpers;
using HIDAeroService.Constants;


namespace HIDAeroService.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class accessLevelController : ControllerBase
    {
        private readonly IAccessLevelService _accessLevelService;
        public accessLevelController(IAccessLevelService accessLevelService)
        {
            _accessLevelService = accessLevelService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<BaseResponse<IEnumerable<AccessLevelDto>>>> GetAll()
        {
            IEnumerable<AccessLevelDto> list = await _accessLevelService.GetAll();
            if (list.Count() > 0)
            {
                return Ok(Helper.ResponseBuilder<IEnumerable<AccessLevelDto>>(HttpStatusCode.OK,ConstantsHelper.SUCCESS,list));
            }
            return NotFound(Helper.ResponseBuilder(HttpStatusCode.NotFound, ConstantsHelper.SUCCESS));
        }

        [HttpPost("tz/{id}")]
        public async Task<ActionResult<BaseResponse<AccessLevelTimeZoneDto>>> GetById(short id)
        {
            AccessLevelTimeZoneDto data = await _accessLevelService.GetTimeZone(id);
            if (data == null)
            {
                return NotFound(Helper.ResponseBuilder(HttpStatusCode.NotFound,ConstantsHelper.NOT_FOUND));
            }
            return Ok(Helper.ResponseBuilder<AccessLevelTimeZoneDto>(HttpStatusCode.OK,ConstantsHelper.SUCCESS,data));
        }

        [HttpPost("add")]
        public async Task<ActionResult<BaseResponse<AccessLevelDto>>> Create(CreateAccessLevelDto dto)
        {
            var data = await _accessLevelService.Create(dto);
            if (data == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, Helper.ResponseBuilder(HttpStatusCode.InternalServerError, ConstantsHelper.INTERNAL_ERROR));
            }
            return StatusCode((int)HttpStatusCode.Created,Helper.ResponseBuilder<AccessLevelDto>(HttpStatusCode.Created, ConstantsHelper.CREATED,data));
        }

        [HttpPost("delete/{accessLevelNo}")]
        public async Task<ActionResult<BaseResponse<AccessLevelDto>>> Remove(short accessLevelNo)
        {
            var data = await _accessLevelService.Remove(accessLevelNo);
            if (data == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, Helper.ResponseBuilder(HttpStatusCode.InternalServerError, ConstantsHelper.INTERNAL_ERROR));
            }
            return Ok(Helper.ResponseBuilder<AccessLevelDto>(HttpStatusCode.OK, ConstantsHelper.REMOVE_SUCCESS, data));
        }

    }
}
