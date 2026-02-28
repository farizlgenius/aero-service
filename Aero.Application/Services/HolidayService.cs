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
    public class HolidayService(IHwRepository hw,IHolCommand hol,IHolRepository repo,ISettingRepository setting) : IHolidayService
    {

        public async Task<ResponseDto<IEnumerable<HolidayDto>>> GetAsync()
        {
            var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<HolidayDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> ClearAsync()
        {
            List<string> errors = new List<string>();
            var ids = await hw.GetComponentIdsAsync();
            foreach (var id in ids)
            {
                if (!hol.ClearHolidayConfiguration(id))
                {
                   errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync(id), Command.HOL_CONFIG));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            var status = await repo.RemoveAllAsync();
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<HolidayDto>> GetByIdAsync(int id)
        {
            var dto = await repo.GetByIdAsync(id);
            if (dto == null) return ResponseHelper.NotFoundBuilder<HolidayDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }



        public async Task<ResponseDto<HolidayDto>> CreateAsync(HolidayDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if (await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<HolidayDto>();

            var ScpSetting = await setting.GetScpSettingAsync();

            List<string> errors = new List<string>();

            if (await repo.IsAnyWithSameDataAsync(dto.Day,dto.Month,dto.Year)) return ResponseHelper.Duplicate<HolidayDto>();

            var Driver = await repo.GetLowestUnassignedNumberAsync(ScpSetting.nHol);
            if (Driver == -1) return ResponseHelper.ExceedLimit<HolidayDto>();


            var domain = new Holiday(Driver,dto.Name,dto.Year,dto.Month,dto.Day,dto.Extend,dto.TypeMask,dto.LocationId,dto.IsActive);

            // Send command 
            var ids = await hw.GetComponentIdsAsync();

            foreach (var id in ids)
            {
                if (!hol.HolidayConfiguration(domain, id))
                {
                   errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync(id), Command.HOL_CONFIG));

                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<HolidayDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<HolidayDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        }

        public async Task<ResponseDto<HolidayDto>> DeleteAsync(int id)
        {
            List<string> errors = new List<string>();

            var en = await repo.GetByIdAsync(id);
            
            if (en is null) return ResponseHelper.NotFoundBuilder<HolidayDto>();
            // Send command 

            var domain = new Holiday(en.DriverId, en.Name, en.Year, en.Month, en.Day, en.Extend, en.TypeMask, en.LocationId, en.IsActive);
            var ids = await hw.GetComponentIdsAsync();

            foreach (var i in ids)
            {
                if (!hol.DeleteHolidayConfiguration(domain, i))
                {
                   errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync(i), Command.HOL_CONFIG));
                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<HolidayDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            
            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<HolidayDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(en);
        }

        public async Task<ResponseDto<HolidayDto>> UpdateAsync(HolidayDto dto)
        {
            List<string> errors = new List<string>();
            var en = repo.IsAnyById(dto.Id);
            if (en is null) return ResponseHelper.NotFoundBuilder<HolidayDto>();

            if (await repo.IsAnyWithSameDataAsync(dto.Day,dto.Month,dto.Year)) return ResponseHelper.Duplicate<HolidayDto>();

            // Send command 
            var ids = await hw.GetComponentIdsAsync();

            var domain = HolidayMapper.ToDomain(dto);

            foreach (var id in ids)
            {
                if (!hol.HolidayConfiguration(domain, id))
                {
                   errors.Add(MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync(id), Command.HOL_CONFIG));
                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<HolidayDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            var status = await repo.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<HolidayDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<HolidayDto>>> DeleteRangeAsync(List<int> ids)
        {
            bool flag = true;
            List<HolidayDto> data = new List<HolidayDto>();
            foreach (var dto in ids)
            {
                var re = await DeleteAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<HolidayDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<HolidayDto>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<HolidayDto>>> GetByLocationAsync(int location)
        {
            var dtos = await repo.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<HolidayDto>>(dtos);
        }

        public async Task<ResponseDto<Pagination<HolidayDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await repo.GetPaginationAsync(param, location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
