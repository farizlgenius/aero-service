
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Application.Services
{
    public class TimeZoneService(ITzRepository repo, IHwRepository hw, ITzCommand tz, ITzRepository rTz,IRunningNumberRepository run,ISettingRepository setting) : ITimeZoneService
    {
        public async Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetAsync()
        {
            var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<TimeZoneDto>>(dtos);
        }

        public async Task<ResponseDto<TimeZoneDto>> GetByIdAsync(int id)
        {
            var dto = await repo.GetByIdAsync(id);

            if (dto is null) return ResponseHelper.NotFoundBuilder<TimeZoneDto>();
            return ResponseHelper.SuccessBuilder<TimeZoneDto>(dto);
        }

        public async Task<ResponseDto<TimeZoneDto>> CreateAsync(CreateTimeZoneDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if (await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<TimeZoneDto>();

            var ScpSetting = await setting.GetScpSettingAsync();


            List<string> errors = new List<string>();
            var DriverId = await repo.GetLowestUnassignedNumberAsync(ScpSetting.nTz);
            if (DriverId == -1) return ResponseHelper.ExceedLimit<TimeZoneDto>();

            var domain = new Aero.Domain.Entities.TimeZone(DriverId,dto.Name,dto.Mode,dto.Active,dto.Deactive,dto.LocationId,dto.IsActive);


            var ids = await hw.GetDriverIdByLocationIdAsync(domain.LocationId);

            foreach (var id in ids)
            {
                if (!tz.ExtendedTimeZoneActSpecification(id, domain))
                {
                    errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync(id), Command.TIMEZONE_SPEC));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<TimeZoneDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);


            var record = await rTz.AddAsync(domain);

            if (record <= 0) return ResponseHelper.UnsuccessBuilder<TimeZoneDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, errors);

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(record));
        }

        public async Task<ResponseDto<TimeZoneDto>> DeleteByIdAsync(int ids)
        {
            List<string> errors = new List<string>();

            var data = await repo.GetByIdAsync(ids);

            if(data is null) return ResponseHelper.NotFoundBuilder<TimeZoneDto>();

            var hws = await hw.GetComponentIdsAsync();
            foreach (var id in hws)
            {
                if (!tz.TimeZoneControl(id, id, 3))
                {
                    errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync(id), Command.TZ_CONTROL));
                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<TimeZoneDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await rTz.DeleteByIdAsync(ids);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<TimeZoneDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS, errors);

            return ResponseHelper.SuccessBuilder<TimeZoneDto>(data);
        }

        public async Task<ResponseDto<TimeZoneDto>> UpdateAsync(TimeZoneDto dto)
        {
            List<string> errors = new List<string>();

            if (!await repo.IsAnyById(dto.Id)) return ResponseHelper.NotFoundBuilder<TimeZoneDto>();

            var domain = new Domain.Entities.TimeZone(dto.DriverId,dto.Name,dto.Mode,dto.Active,dto.Deactive,dto.LocationId,dto.IsActive);

            var ids = await hw.GetComponentIdsAsync();
            foreach (var id in ids)
            {
                if (!tz.ExtendedTimeZoneActSpecification(id, domain))
                {
                    errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync(id), Command.TIMEZONE_SPEC));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<TimeZoneDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var data = TimezoneMapper.ToDomain(dto);

            var status = await rTz.UpdateAsync(data);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<TimeZoneDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS, errors);

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(domain.Id));
        }


        public async Task<ResponseDto<IEnumerable<Mode>>> GetModeAsync(int param)
        {
            var dtos = await repo.GetModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync()
        {
            var dtos = await repo.GetCommandAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetByLocationAsync(short location)
        {
            var dtos = await repo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<TimeZoneDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<TimeZoneDto>>> DeleteRangeAsync(List<short> components)
        {
            bool flag = true;
            List<TimeZoneDto> data = new List<TimeZoneDto>();
            foreach (var dto in components)
            {
                var re = await DeleteByIdAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<TimeZoneDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<TimeZoneDto>>(data);

            return res;
        }

        public async Task<ResponseDto<Pagination<TimeZoneDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await repo.GetPaginationAsync(param, location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
