using System.Net;
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Interface;

namespace Aero.Application.Services
{
    public sealed class MonitorPointService(IQMpRepository qMp,IMpCommand mp,IQHwRepository qHw,IMpRepository rMp) : IMonitorPointService
    {

        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetAsync()
        {
            var dtos = await qMp.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByLocationAsync(short location)
        {
            var dtos = await qMp.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<short>>> GetAvailableIp(string mac, short sio)
        {
            var res = await qMp.GetAvailableIpAsync(mac,sio);
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(res);
        }




        public async Task<ResponseDto<bool>> MaskAsync(MonitorPointDto dto, bool IsMask)
        {

            if (!await qMp.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await qHw.GetComponentFromMacAsync(dto.Mac);

            if (!mp.MonitorPointMask(ScpId,dto.ComponentId, IsMask ? 1 : 0))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.SET_MASK));
            }

            var status = await rMp.SetMaskAsync(dto.Mac,dto.MpId,IsMask);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<bool>> CreateAsync(MonitorPointDto dto)
        {
            
            var componentId = await qMp.GetLowestUnassignedNumberAsync(10,"");
            if (componentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var mpid = await qMp.GetLowestUnassignedNumberAsync(10,dto.Mac);

            var ScpId = await qHw.GetComponentFromMacAsync(dto.Mac);

            var domain = MonitorPointMapper.ToDomain(dto);
            domain.MpId = mpid;
            domain.ComponentId = componentId;

            if (!mp.InputPointSpecification(ScpId, dto.ModuleId, dto.InputNo, dto.InputMode, dto.Debounce, dto.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.INPUT_SPEC));
            }


            if (!mp.MonitorPointConfiguration(ScpId, dto.ModuleId, dto.InputNo, dto.LogFunction, dto.MonitorPointMode, dto.DelayEntry, dto.DelayExit, mpid))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.MONITOR_CONFIG));
            }

            var status = await rMp.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<bool>> DeleteAsync(short ComponentId)
        {
            if (!await qMp.IsAnyByComponentId(ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            var data = await qMp.GetByComponentIdAsync(ComponentId);
            var ScpId = await qHw.GetComponentFromMacAsync(data.Mac);

            if (!mp.MonitorPointConfiguration(ScpId,-1, data.InputNo, data.LogFunction, data.MonitorPointMode, data.DelayEntry, data.DelayExit, ComponentId))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(data.Mac, Command.MONITOR_CONFIG));
            }

            var status = await rMp.DeleteByComponentIdAsync(ComponentId);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);
            
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<MonitorPointDto>> UpdateAsync(MonitorPointDto dto)
        {

            if (!await qMp.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<MonitorPointDto>();

            var ScpId = await qHw.GetComponentFromMacAsync(dto.Mac);

            var domain = MonitorPointMapper.ToDomain(dto);

            if (!mp.InputPointSpecification(ScpId, domain.ModuleId, domain.InputNo, domain.InputMode, domain.Debounce, domain.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.INPUT_SPEC));
            }

            if (!mp.MonitorPointConfiguration(ScpId, domain.ModuleId, domain.InputNo, domain.LogFunction, domain.MonitorPointMode, domain.DelayEntry, domain.DelayExit, domain.MpId))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.MONITOR_CONFIG));
            }

            var status = await rMp.UpdateAsync(domain);
            return ResponseHelper.SuccessBuilder<MonitorPointDto>(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusByComponentIdAsync(short component)
        {
            var mac = await qMp.GetMacFromComponentIdAsync(component);
            var ScpId = await qHw.GetComponentFromMacAsync(mac);
            if(ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!mp.GetMpStatus(ScpId, component, 1))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.MP_STATUS));
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            switch (param)
            {
                case 0:
                    var dtos = await qMp.GetInputModeAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case 1:
                    var d = await qMp.GetMonitorPointModeAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(d);
                default:
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>([]);

            }

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

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetLogFunctionAsync()
        {
            
            var dtos = await qMp.GetLogFunctionAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

            public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByMacAsync(string mac)
            {
                var dtos = await qMp.GetByMacAsync(mac);
                  return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
            }
      }
}
