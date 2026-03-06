

using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Application.Services
{
    public sealed class TriggerService(ITriggerRepository repo, IDeviceRepository hw, ITrigCommand trig,ISettingRepository setting) : ITriggerService
    {
        public async Task<ResponseDto<TriggerDto>> CreateAsync(TriggerDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if (await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<TriggerDto>();

            var ScpSetting = await setting.GetScpSettingAsync();

            var DriverId = await repo.GetLowestUnassignedNumberAsync(ScpSetting.nTrgr,dto.DeviceId);
            if (DriverId == -1) return ResponseHelper.ExceedLimit<TriggerDto>();


            var domain = new Aero.Domain.Entities.Trigger(DriverId,dto.Name,dto.Command,dto.ProcedureId,dto.SourceType,dto.SourceNumber,dto.TranType,
            dto.CodeMap.Select(x => new TransactionCode(x.Name,x.Value,x.Description)).ToList(),
            dto.TimeZone,dto.DriverId,dto.LocationId,dto.IsActive);

            if (!trig.TriggerSpecification((short)dto.DeviceId, domain, dto.DriverId))
            {
                return ResponseHelper.UnsuccessBuilderWithString<TriggerDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)dto.DeviceId), Command.TRIG_SPEC));
            }

            var status = await repo.AddAsync(domain);
            if (status <= 0) return ResponseHelper.UnsuccessBuilder<TriggerDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

            return ResponseHelper.SuccessBuilder<TriggerDto>(await repo.GetByIdAsync(status));
        }

        public async Task<ResponseDto<TriggerDto>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<TriggerDto>>> GetAsync()
        {
            var dto = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<TriggerDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<TriggerDto>>> GetByLocationId(int location)
        {
            var dto = await repo.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<TriggerDto>>(dto);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync()
        {
            var dtos = await repo.GetCommandAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetSourceTypeAsync()
        {
            var dtos = await repo.GetSourceTypeAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCodeByTranAsync(short tran)
        {
            var dtos = await repo.GetCodeByTranAsync(tran);

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetTypeBySourceAsync(short source)
        {
            var dtos = await repo.GetTypeBySourceAsync(source);

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);


        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetDeviceBySourceAsync(short location, short source)
        {
            var dtos = await repo.GetDeviceBySourceAsync(location, source);

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public Task<ResponseDto<TriggerDto>> UpdateAsync(TriggerDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<Pagination<TriggerDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await repo.GetPaginationAsync(param, location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
