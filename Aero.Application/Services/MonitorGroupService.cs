

using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Application.Services
{
    public sealed class MonitorGroupService(IMpgCommand mpg,IDeviceRepository hw,IMpgRepository repo,ISettingRepository setting) : IMonitorGroupService
    {
        public async Task<ResponseDto<MonitorGroupDto>> CreateAsync(MonitorGroupDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if(await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<MonitorGroupDto>();

            var ScpSetting = await setting.GetScpSettingAsync();

            var DriverId = await repo.GetLowestUnassignedNumberAsync(ScpSetting.nMpg,dto.DeviceId);
            if (DriverId == -1) return ResponseHelper.ExceedLimit<MonitorGroupDto>();

            var domain = new MonitorGroup(
                dto.DeviceId,
                DriverId,
                dto.Name,
                dto.nMpCount,
                dto.nMpList.Select(x => new MonitorGroupList(DriverId,x.PointType,x.PointTypeDesc,x.PointNumber)).ToList(),
                dto.LocationId,
                dto.IsActive
                );


            if (!mpg.ConfigureMonitorPointGroup((short)dto.DeviceId, DriverId, dto.nMpCount, domain.nMpList.ToList()))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorGroupDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.CONFIG_MPG));
            }

            var status = await repo.AddAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<MonitorGroupDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<MonitorGroupDto>(await repo.GetByIdAsync(status));
        }

        public async Task<ResponseDto<MonitorGroupDto>> DeleteAsync(int id)
        {
            var en = await repo.GetByIdAsync(id);
            
            if(en is null) return ResponseHelper.NotFoundBuilder<MonitorGroupDto>();

            if (!mpg.ConfigureMonitorPointGroup((short)en.DeviceId, en.DriverId, 0, []))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorGroupDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)en.DeviceId), Command.CONFIG_MPG));
            }

            var status = await repo.DeleteByIdAsync(id);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<MonitorGroupDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(en);
        }

        public async Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetAsync()
        {
            var dto = await repo.GetAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorGroupDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetByLocationAsync(short location)
        {
            var dto = await repo.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorGroupDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync()
        {
            var dtos = await repo.GetCommandAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);

        }

        public async Task<ResponseDto<Pagination<MonitorGroupDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetTypeAsync()
        {
            var dtos = await repo.GetTypeAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> MonitorGroupCommandAsync(MonitorGroupCommandDto dto)
        {
            var en = await repo.GetByIdAsync(dto.Id);
            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            if (!mpg.MonitorPointGroupArmDisarm((short)en.DeviceId, en.DriverId, dto.Command, dto.Arg))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)en.DeviceId), Command.MPG_ARM_DISARM));
            }

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<MonitorGroupDto>> UpdateAsync(MonitorGroupDto dto)
        {

            if (!await repo.IsAnyByIdAsync(dto.Id)) return ResponseHelper.NotFoundBuilder<MonitorGroupDto>();


            // Delete relate table first 
            var status = await repo.DeleteReferenceByIdAsync(dto.Id);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<MonitorGroupDto>(ResponseMessage.REMOVE_OLD_REF_UNSUCCESS,[]);

             var domain = new MonitorGroup(
                dto.DeviceId,
                dto.DriverId,
                dto.Name,
                dto.nMpCount,
                dto.nMpList.Select(x => new MonitorGroupList(dto.DriverId,x.PointType,x.PointTypeDesc,x.PointNumber)).ToList(),
                dto.LocationId,
                dto.IsActive
                );

            if (!mpg.ConfigureMonitorPointGroup((short)domain.DeviceId, dto.DriverId, dto.nMpCount, domain.nMpList.ToList()))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorGroupDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.CONFIG_MPG));
            }


            status = await repo.UpdateAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<MonitorGroupDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<MonitorGroupDto>(dto);

        }
    }
}
