using Aero.Api.Constants;
using Aero.Application.Commands.Interfaces;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Application.Services
{
    public sealed class ControlPointService(IDeviceRepository hw,IAeroAdapter cp,ICpRepository repo,ISettingRepository setting) : IControlPointService 
    {
        public async Task<ResponseDto<IEnumerable<ControlPointDto>>> GetAsync()
        {
            var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ControlPointDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ControlPointDto>>> GetByLocationAsync(int location)
        {
            var dtos = await repo.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<ControlPointDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> GetOfflineModeAsync()
        {
            var dtos = await repo.GetOfflineModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        private async Task<ResponseDto<IEnumerable<ModeDto>>> GetRelayModeAsync()
        {
            var dtos = await repo.GetRelayModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<bool>> ToggleAsync(ToggleControlPointDto dto)
        {
            if(!await hw.IsAnyByDriverIdAsync(dto.DeviceId)) return ResponseHelper.NotFoundBuilder<bool>();
            if(!await repo.IsAnyByDeviceIdAndDriverIdAsync(dto.DeviceId,dto.DriverId)) return ResponseHelper.NotFoundBuilder<bool>();
            if (!cp.ControlPointCommand((short)dto.DeviceId, dto.DriverId, dto.Command))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)dto.DeviceId),Command.CP_COMMAND));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<short>>> GetAvailableOpAsync(int ModuleId)
        {
            var res = await repo.GetAvailableOpAsync(ModuleId);
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(res);
        }




        public async Task<ResponseDto<ControlPointDto>> CreateAsync(CreateControlPointDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if (await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<ControlPointDto>();

            if(!await hw.IsAnyByDriverIdAsync(dto.ScpId)) return ResponseHelper.NotFoundBuilder<ControlPointDto>();

            var ScpSetting = await setting.GetScpSettingAsync();

            var DriverId = await repo.GetLowestUnassignedNumberAsync(ScpSetting.nCp,dto.ScpId);

            if (DriverId == -1) return ResponseHelper.ExceedLimit<ControlPointDto>();

            short modeNo = await repo.GetModeNoByOfflineAndRelayModeAsync(dto.OfflineMode,dto.RelayMode);


            var domain = new Aero.Domain.Entities.ControlPoint(0,DriverId,dto.Name,dto.ModuleId,dto.ModuleDriverId,dto.ModuleDetail,dto.OutputNo,dto.RelayMode,dto.RelayModeDetail,dto.OfflineMode,dto.OfflineModeDetail,dto.DefaultPulse,dto.ScpId,dto.LocationId,dto.IsActive);

            if (!cp.OutputPointSpecification((short)dto.ScpId, domain.ModuleDriverId, domain.OutputNo, modeNo))
            {
                return ResponseHelper.UnsuccessBuilderWithString<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.OUTPUT_SPEC));
            }
            

            if (!cp.ControlPointConfiguration((short)dto.ScpId, domain.ModuleDriverId,DriverId, domain.OutputNo, domain.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilderWithString<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.CONTROL_CONFIG));

            }

            var status = await repo.AddAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<ControlPointDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        }

        public async Task<ResponseDto<ControlPointDto>> DeleteAsync(int id)
        {

            var dto = await repo.GetByIdAsync(id);

            if(dto is null) return ResponseHelper.NotFoundBuilder<ControlPointDto>();

            if (!cp.ControlPointConfiguration((short)dto.ScpId, -1, dto.CpId, dto.OutputNo, dto.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilderWithString<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)dto.ScpId), Command.CONTROL_CONFIG));
            }

            var status = await repo.DeleteByIdAsync(id);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<ControlPointDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<ControlPointDto>> UpdateAsync(ControlPointDto dto)
        {

            if (!await repo.IsAnyByIdAsync(dto.Id)) return ResponseHelper.NotFoundBuilder<ControlPointDto>();

            var domain = new Aero.Domain.Entities.ControlPoint(dto.Id,dto.CpId, dto.Name, dto.ModuleId,dto.ModuleDriverId,dto.ModuleDetail, dto.OutputNo, dto.RelayMode, dto.RelayModeDetail, dto.OfflineMode, dto.OfflineModeDetail, dto.DefaultPulse, dto.ScpId, dto.LocationId, dto.IsActive);

            short modeNo = await repo.GetModeNoByOfflineAndRelayModeAsync(domain.OfflineMode,domain.RelayMode);
            if (!cp.OutputPointSpecification((short)domain.CpId, domain.ModuleDriverId, domain.OutputNo, modeNo))
            {
                return ResponseHelper.UnsuccessBuilderWithString<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)dto.ScpId), Command.OUTPUT_SPEC));
            }

            if (!cp.ControlPointConfiguration((short)domain.CpId, domain.ModuleDriverId, domain.CpId, domain.OutputNo, domain.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilderWithString<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)dto.ScpId), Command.CONTROL_CONFIG));
            }


            var status = await repo.UpdateAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<ControlPointDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(int deviceId, int driverId)
        {
            if (!cp.GetCpStatus((short)deviceId, driverId, 1))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)deviceId),Command.CP_STATUS));
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

        public async Task<ResponseDto<ControlPointDto>> GetByIdAsync(int id)
        {
           var dto = await repo.GetByIdAsync(id);

            return ResponseHelper.SuccessBuilder<ControlPointDto>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ControlPointDto>>> DeleteRangeAsync(List<int> components)
        {
            bool flag = true;
            List<ControlPointDto> data = new List<ControlPointDto>();
            foreach (var dto in components)
            {
                var re = await DeleteAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ControlPointDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ControlPointDto>>(data);

            return res;
        }

        public async Task<ResponseDto<Pagination<ControlPointDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await repo.GetPaginationAsync(param, location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
