using System.Net;
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
    public sealed class MonitorPointService(IMpRepository repo,IMpCommand mp,IDeviceRepository hw,ISettingRepository setting) : IMonitorPointService
    {

        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetAsync()
        {
            var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByLocationAsync(int location)
        {
            var dtos = await repo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<short>>> GetAvailableIp(int id)
        {
            var res = await repo.GetAvailableIpAsync(id);
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(res);
        }




        public async Task<ResponseDto<bool>> MaskAsync(MonitorPointDto dto, bool IsMask)
        {
            var en = await repo.GetByIdAsync(dto.Id);

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();


            if (!mp.MonitorPointMask((short)en.DeviceId,dto.DriverId, IsMask ? 1 : 0))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)en.DeviceId), Command.SET_MASK));
            }

            var status = await repo.SetMaskByIdAsync(dto.Id,IsMask);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<MonitorPointDto>> CreateAsync(MonitorPointDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if(await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<MonitorPointDto>();

            var ScpSetting = await setting.GetScpSettingAsync();

            var DriverId = await repo.GetLowestUnassignedNumberAsync(ScpSetting.nMp,dto.DeviceId);
            if (DriverId == -1) return ResponseHelper.ExceedLimit<MonitorPointDto>();


             var domain = new MonitorPoint(
                dto.Id,
                dto.DeviceId,
                DriverId,
                dto.Name,
                dto.ModuleId,
                dto.ModuleDescription,
                dto.InputNo,
                dto.InputMode,
                dto.InputModeDescription,
                dto.Debounce,
                dto.HoldTime,
                dto.LogFunction,
                dto.LogFunctionDescription,
                dto.MonitorPointMode,
                dto.MonitorPointModeDescription,
                dto.DelayEntry,
                dto.DelayExit,
                dto.IsMask
                );


            if (!mp.InputPointSpecification((short)domain.DeviceId, dto.ModuleId, dto.InputNo, dto.InputMode, dto.Debounce, dto.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.INPUT_SPEC));
            }


            if (!mp.MonitorPointConfiguration((short)domain.DeviceId, dto.ModuleId, dto.InputNo, dto.LogFunction, dto.MonitorPointMode, dto.DelayEntry, dto.DelayExit, DriverId))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.MONITOR_CONFIG));
            }

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<MonitorPointDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        }


        public async Task<ResponseDto<MonitorPointDto>> DeleteAsync(int Id)
        {
            var en = await repo.GetByIdAsync(Id);
            if (en is null) return ResponseHelper.NotFoundBuilder<MonitorPointDto>();

            if (!mp.MonitorPointConfiguration((short)en.DeviceId,-1, en.InputNo, en.LogFunction, en.MonitorPointMode, en.DelayEntry, en.DelayExit, en.DriverId))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)en.DeviceId), Command.MONITOR_CONFIG));
            }

            var status = await repo.DeleteByIdAsync(Id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<MonitorPointDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);
            
            return ResponseHelper.SuccessBuilder(en);
        }

        public async Task<ResponseDto<MonitorPointDto>> UpdateAsync(MonitorPointDto dto)
        {
            var en = await repo.GetByIdAsync(dto.Id);
            if (en is null) return ResponseHelper.NotFoundBuilder<MonitorPointDto>();


            var domain = new MonitorPoint(
                dto.Id,
                dto.DeviceId,
                dto.DriverId,
                dto.Name,
                dto.ModuleId,
                dto.ModuleDescription,
                dto.InputNo,
                dto.InputMode,
                dto.InputModeDescription,
                dto.Debounce,
                dto.HoldTime,
                dto.LogFunction,
                dto.LogFunctionDescription,
                dto.MonitorPointMode,
                dto.MonitorPointModeDescription,
                dto.DelayEntry,
                dto.DelayExit,
                dto.IsMask
                );

            if (!mp.InputPointSpecification((short)domain.DeviceId, domain.ModuleId, domain.InputNo, domain.InputMode, domain.Debounce, domain.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.INPUT_SPEC));
            }

            if (!mp.MonitorPointConfiguration((short)domain.DeviceId, domain.ModuleId, domain.InputNo, domain.LogFunction, domain.MonitorPointMode, domain.DelayEntry, domain.DelayExit, domain.DriverId))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.MONITOR_CONFIG));
            }

            var status = await repo.UpdateAsync(domain);
            return ResponseHelper.SuccessBuilder<MonitorPointDto>(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusByIdAsync(int id)
        {
            var en = await repo.GetByIdAsync(id);
            if(en is null) return ResponseHelper.NotFoundBuilder<bool>();
            if (!mp.GetMpStatus((short)en.DeviceId, en.DriverId, 1))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)en.DeviceId),Command.MP_STATUS));
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            switch (param)
            {
                case 0:
                    var dtos = await repo.GetInputModeAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case 1:
                    var d = await repo.GetMonitorPointModeAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(d);
                default:
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>([]);

            }

        }



        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> DeleteRangeAsync(List<int> ids)
        {
            bool flag = true;
            List<MonitorPointDto> data = new List<MonitorPointDto>();
            foreach (var dto in ids)
            {
                var re = await DeleteAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<MonitorPointDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetLogFunctionAsync()
        {
            
            var dtos = await repo.GetLogFunctionAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

            public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByDviceIdAsync(int id)
            {
                var dtos = await repo.GetByDeviceIdAsync(id);
                  return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
            }

            public async Task<ResponseDto<MonitorPointDto>> GetByIdAsync(int id)
            {
                  var dtos = await repo.GetByIdAsync(id);
                  return ResponseHelper.SuccessBuilder<MonitorPointDto>(dtos);
            }

        public async Task<ResponseDto<Pagination<MonitorPointDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await repo.GetPaginationAsync(param, location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
