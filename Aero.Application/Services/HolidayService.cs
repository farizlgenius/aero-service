using System.Net;
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

namespace Aero.Application.Services
{
    public class HolidayService(IQHolRepository qHol,IQHwRepository qHw,IHolCommand hol,IHolRepository rHol) : IHolidayService
    {

        public async Task<ResponseDto<IEnumerable<HolidayDto>>> GetAsync()
        {
            var dtos = await qHol.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<HolidayDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> ClearAsync()
        {
            List<string> errors = new List<string>();
            var ids = await qHw.GetComponentIdsAsync();
            foreach (var id in ids)
            {
                if (!hol.ClearHolidayConfiguration(id))
                {
                   errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id), Command.HOL_CONFIG));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            var status = await rHol.RemoveAllAsync();
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<HolidayDto>> GetByComponentIdAsync(short component)
        {
            var dto = await qHol.GetByComponentIdAsync(component);
            if (dto == null) return ResponseHelper.NotFoundBuilder<HolidayDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }



        public async Task<ResponseDto<bool>> CreateAsync(HolidayDto dto)
        {
            List<string> errors = new List<string>();

            if (await qHol.IsAnyWithSameDataAsync(dto.Day,dto.Month,dto.Year)) return ResponseHelper.Duplicate<bool>();

            var ComponentId = await qHol.GetLowestUnassignedNumberAsync(10,"");
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            dto.ComponentId = ComponentId;

            var domain = HolidayMapper.ToDomain(dto);

            // Send command 
            var ids = await qHw.GetComponentIdsAsync();

            foreach (var id in ids)
            {
                if (!hol.HolidayConfiguration(domain, id))
                {
                   errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id), Command.HOL_CONFIG));

                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await rHol.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            List<string> errors = new List<string>();

            var domain = await rHol.GetByComponentIdAsync(component);
            
            if (!await qHol.IsAnyByComponentId(component)) return ResponseHelper.NotFoundBuilder<bool>();
            // Send command 
           var ids = await qHw.GetComponentIdsAsync();

            foreach (var id in ids)
            {
                if (!hol.DeleteHolidayConfiguration(domain, id))
                {
                   errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id), Command.HOL_CONFIG));
                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            
            var status = await rHol.DeleteByComponentIdAsync(component);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<HolidayDto>> UpdateAsync(HolidayDto dto)
        {
            List<string> errors = new List<string>();
            if (!await qHol.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<HolidayDto>();

            if (await qHol.IsAnyWithSameDataAsync(dto.Day,dto.Month,dto.Year)) return ResponseHelper.Duplicate<HolidayDto>();

            // Send command 
            var ids = await qHw.GetComponentIdsAsync();

            var domain = HolidayMapper.ToDomain(dto);

            foreach (var id in ids)
            {
                if (!hol.HolidayConfiguration(domain, id))
                {
                   errors.Add(MessageBuilder.Unsuccess(await qHw.GetMacFromComponentAsync(id), Command.HOL_CONFIG));
                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<HolidayDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            var status = await rHol.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<HolidayDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach (var dto in components)
            {
                var re = await DeleteAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<HolidayDto>>> GetByLocationAsync(short location)
        {
            var dtos = await qHol.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<HolidayDto>>(dtos);
        }

        public async Task<ResponseDto<Pagination<HolidayDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await qHol.GetPaginationAsync(param, location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
