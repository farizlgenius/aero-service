
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Application.Services
{
    public class TimeZoneService(IQTzRepository qTz,IQHwRepository qHw,ITzCommand tz,ITzRepository rTz) : ITimeZoneService
    {
        public async Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetAsync()
        {
            var dtos = await qTz.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<TimeZoneDto>>(dtos);
        }

        public async Task<ResponseDto<TimeZoneDto>> GetByComponentIdAsync(short component)
        {
            var dto = await qTz.GetByComponentIdAsync(component);

            if (dto is null) return ResponseHelper.NotFoundBuilder<TimeZoneDto>();
            return ResponseHelper.SuccessBuilder<TimeZoneDto>(dto);
        }

        public async Task<ResponseDto<bool>> CreateAsync(TimeZoneDto dto)
        {
            List<string> errors = new List<string>();
            var ComponentId = await qTz.GetLowestUnassignedNumberAsync(10);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            dto.ComponentId = ComponentId;

            var timezone = TimezoneMapper.ToDomain(dto);
            
            var ids = await qHw.GetComponentIdsAsync();

            foreach (var id in ids)
            {
                long active = UtilitiesHelper.DateTimeToElapeSecond(dto.ActiveTime);
                long deactive = UtilitiesHelper.DateTimeToElapeSecond(dto.DeactiveTime);
                if (!tz.ExtendedTimeZoneActSpecification(id, timezone,timezone.Intervals is null ? [] : timezone.Intervals , (int)active, (int)deactive))
                {
                   errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id), Command.TIMEZONE_SPEC));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);


            var status = await rTz.AddAsync(timezone);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,errors);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            List<string> errors = new List<string>();
            
            if(!await qTz.IsAnyByComponentId(component)) return ResponseHelper.NotFoundBuilder<bool>();

            var hw = await qHw.GetComponentIdsAsync();
            foreach(var id in hw)
            {
                if (!tz.TimeZoneControl(id,component,3))
                {
                   errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id),Command.TZ_CONTROL));
                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            var status = await rTz.DeleteByComponentIdAsync(component);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,errors);

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<TimeZoneDto>> UpdateAsync(TimeZoneDto dto)
        {
            List<string> errors = new List<string>();

            if (!await qTz.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<TimeZoneDto>();

            var domain = TimezoneMapper.ToDomain(dto);

            var ids = await qHw.GetComponentIdsAsync();
            foreach (var id in ids)
            {
                long active = UtilitiesHelper.DateTimeToElapeSecond(dto.ActiveTime);
                long deactive = UtilitiesHelper.DateTimeToElapeSecond(dto.DeactiveTime);
                if (!tz.ExtendedTimeZoneActSpecification(id, domain, domain.Intervals.ToList(), (int)active, (int)deactive))
                {
                   errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id), Command.TIMEZONE_SPEC));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<TimeZoneDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            var data = TimezoneMapper.ToDomain(dto);

            var status = await rTz.UpdateAsync(data);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<TimeZoneDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,errors);

            return ResponseHelper.SuccessBuilder(dto);
        }


        public async Task<ResponseDto<IEnumerable<Mode>>> GetModeAsync(int param)
        {
            var dtos = await qTz.GetModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync()
        {
            var dtos = await qTz.GetCommandAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetByLocationAsync(short location)
        {
            var dtos = await qTz.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<TimeZoneDto>>(dtos);
        }
    }
}
