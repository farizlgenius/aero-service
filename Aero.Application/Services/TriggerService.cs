

using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Application.Services
{
    public sealed class TriggerService(IQTrigRepository qTrig,ITriggerRepository rTrig,IQHwRepository qHw,ITrigCommand trig) : ITriggerService
    {
        public async Task<ResponseDto<bool>> CreateAsync(TriggerDto dto)
        {
            var ComponentId = await qTrig.GetLowestUnassignedNumberAsync(128,"");
            var TrigId = await qTrig.GetLowestUnassignedNumberAsync(128,dto.Mac);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();
            var ScpId = await qHw.GetComponentIdFromMacAsync(dto.Mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();

            var domain = TriggerMapper.ToDomain(dto);
            domain.TrigId = TrigId;
            domain.ComponentId = ComponentId;

            if(!trig.TriggerSpecification(ScpId,domain,ComponentId))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.TRIG_SPEC));
            }

            var status = await rTrig.AddAsync(domain);
            if(status <= 0)return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string Mac, short ComponentId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<TriggerDto>>> GetAsync()
        {
            var dto = await qTrig.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<TriggerDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<TriggerDto>>> GetByLocationId(short location)
        {
            var dto = await qTrig.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<TriggerDto>>(dto);
        }


        public async Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync()
        {
            var dtos = await qTrig.GetCommandAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetSourceTypeAsync()
        {
            var dtos = await qTrig.GetSourceTypeAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetCodeByTranAsync(short tran)
        {
            var dtos = await qTrig.GetCodeByTranAsync(tran);

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetTypeBySourceAsync(short source)
        {
            var dtos = await qTrig.GetTypeBySourceAsync(source);

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);


        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetDeviceBySourceAsync(short location, short source)
        {
            var dtos = await rTrig.GetDeviceBySourceAsync(location,source);

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public Task<ResponseDto<TriggerDto>> UpdateAsync(TriggerDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<Pagination<TriggerDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await qTrig.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
