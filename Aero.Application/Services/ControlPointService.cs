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
    public sealed class ControlPointService(IQCpRepository qCp,IQHwRepository qHw,ICpCommand cp,ICpRepository rCp) : IControlPointService 
    {
        public async Task<ResponseDto<IEnumerable<ControlPointDto>>> GetAsync()
        {
            var dtos = await qCp.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ControlPointDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ControlPointDto>>> GetByLocationAsync(short location)
        {
            var dtos = await qCp.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<ControlPointDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> GetOfflineModeAsync()
        {
            var dtos = await qCp.GetOfflineModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        private async Task<ResponseDto<IEnumerable<ModeDto>>> GetRelayModeAsync()
        {
            var dtos = await qCp.GetRelayModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<bool>> ToggleAsync(ToggleControlPointDto dto)
        {
            List<string> errors = new List<string>();
            var id = await qHw.GetComponentFromMacAsync(dto.Mac);
            if(id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!cp.ControlPointCommand(id, dto.ComponentId, dto.Command))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(dto.Mac,Command.CP_COMMAND));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<short>>> GetAvailableOpAsync(string mac, short ModuleId)
        {
            var res = await qCp.GetAvailableOpAsync(mac,ModuleId);
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(res);
        }




        public async Task<ResponseDto<bool>> CreateAsync(ControlPointDto dto)
        {
            var componentId = await qCp.GetLowestUnassignedNumberAsync(10);

            if (componentId == -1) return ResponseHelper.ExceedLimit<bool>();

            short modeNo = await qCp.GetModeNoByOfflineAndRelayModeAsync(dto.OfflineMode,dto.RelayMode);
            
            var ScpId = await qHw.GetComponentFromMacAsync(dto.Mac);

            var domain = ControlPointMapper.ToDomain(dto);

            if (!cp.OutputPointSpecification(ScpId, domain.ModuleId, domain.OutputNo, modeNo))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.OUTPUT_SPEC));
            }
            

            if (!cp.ControlPointConfiguration(ScpId, domain.ModuleId, (short)componentId, domain.OutputNo, domain.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.CONTROL_CONFIG));

            }

            var status = await rCp.AddAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {

            if (!await qCp.IsAnyByComponentId(component)) return ResponseHelper.NotFoundBuilder<bool>();

            var dto = await qCp.GetByComponentIdAsync(component);

            var scpId = await qHw.GetComponentFromMacAsync(dto.Mac);

            if (!cp.ControlPointConfiguration(scpId, -1, (short)dto.ComponentId, dto.OutputNo, dto.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(dto.Mac,Command.CONTROL_CONFIG));
            }

            var status = await rCp.DeleteByComponentIdAsync(component);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<ControlPointDto>> UpdateAsync(ControlPointDto dto)
        {

            if (!await qCp.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<ControlPointDto>();

            var domain = ControlPointMapper.ToDomain(dto);
            var scpId = await qHw.GetComponentFromMacAsync(domain.Mac);
            short modeNo = await qCp.GetModeNoByOfflineAndRelayModeAsync(domain.OfflineMode,domain.RelayMode);
            if (!cp.OutputPointSpecification(scpId, domain.ModuleId, domain.OutputNo, modeNo))
            {
                return ResponseHelper.UnsuccessBuilderWithString<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.OUTPUT_SPEC));
            }

            if (!cp.ControlPointConfiguration(scpId, domain.ModuleId, (short)domain.ComponentId, domain.OutputNo, domain.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilderWithString<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.CONTROL_CONFIG));
            }


            var status = await rCp.UpdateAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<ControlPointDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short component)
        {
            var id = await qHw.GetComponentFromMacAsync(mac);
            if (!cp.GetCpStatus(id, component, 1))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.CP_STATUS));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            switch (param)
            {
                case 0:
                    return await GetOfflineModeAsync();
                case 1:
                    return await GetRelayModeAsync();
                default:
                    return ResponseHelper.NotFoundBuilder<IEnumerable<ModeDto>>();
            }
        }

        public async Task<ResponseDto<ControlPointDto>> GetByMacAndIdAsync(string Mac, short ComponentId)
        {
           var dto = await qCp.GetByMacAndComponentIdAsync(Mac,ComponentId);

            return ResponseHelper.SuccessBuilder<ControlPointDto>(dto);
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
    }
}
